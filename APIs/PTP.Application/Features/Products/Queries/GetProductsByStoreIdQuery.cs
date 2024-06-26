using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Queries;

public class GetProductsByStoreIdQuery : IRequest<Pagination<ProductViewModel>>
{
    public Guid StoreId { get; set; } = default!;

    public Guid? MenuId { get; set; }
    //public Guid CategoryId{get;set;}=default!;

    public Dictionary<string, string>? Filter { get; set; } = default!;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }


    public class QueryValidation : AbstractValidator<GetProductsByStoreIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }
    public class QueryHandler : IRequestHandler<GetProductsByStoreIdQuery, Pagination<ProductViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private ILogger<QueryHandler> _logger;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task<Pagination<ProductViewModel>> Handle(GetProductsByStoreIdQuery request, CancellationToken cancellationToken)
        {
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");
            if (request.Filter.ContainsKey("menuId"))
            {
                request.Filter!.Remove("menuId");
            }


            var cacheResult = await GetCache(request);

            var products = cacheResult != null ? cacheResult : await _unitOfWork.ProductRepository.WhereAsync(x => x.StoreId == request.StoreId, x => x.Store, x => x.Category, x => x.ProductInMenus);
            if (products is null) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any products!");
            await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, products);
            var viewModels = _mapper.Map<List<ProductViewModel>>(products);

            for (int i = 0; i < viewModels.Count; i++)
            {
                viewModels[i].ProductMenuId = products[i].ProductInMenus.First().Id;
                viewModels[i].QuantityInDay = products[i].ProductInMenus.First().QuantityInDay;
                viewModels[i].MenuId = products[i].ProductInMenus.First().MenuId;
                viewModels[i].QuantityUsed = products[i].ProductInMenus.First().QuantityUsed;
                viewModels[i].SalePrice = products[i].ProductInMenus.First().SalePrice;
                var menu = await _cacheService.GetAsync<Menu>(CacheKey.MENU + viewModels[i].MenuId);
                viewModels[i].MenuName = menu != null ? menu.Name : "";
            }
            if (request.MenuId != Guid.Empty) viewModels = viewModels.Where(x => x.MenuId == request.MenuId).ToList();
            var filterResult = request.Filter.Count > 0 ? new List<ProductViewModel>() : viewModels.AsEnumerable();

            if (request.Filter!.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResult = filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                }
            }

            return new Pagination<ProductViewModel>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalItemsCount = filterResult.Count(),
                Items = PaginatedList<ProductViewModel>.Create(
                       source: filterResult.AsQueryable(),
                       pageIndex: request.PageNumber,
                       pageSize: request.PageSize
               )
            };
        }

        public async Task<List<Product>?> GetCache(GetProductsByStoreIdQuery request)
        {

            if (!_cacheService.IsConnected()) return null;

            var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
            if (cacheResult!.Count > 0)
            {
                var result = cacheResult.Where(x => x.StoreId == request.StoreId).ToList();
                return result.Count == 0 ? null : result;
            }
            return null;
        }
    }
}