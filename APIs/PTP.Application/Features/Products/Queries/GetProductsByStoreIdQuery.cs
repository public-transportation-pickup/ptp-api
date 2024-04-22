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
            request.Filter!.Remove("menuId");

            var cacheResult = await GetCache(request);
            if (cacheResult is not null) return cacheResult;

            var products = await _unitOfWork.ProductRepository.WhereAsync(x => x.StoreId == request.StoreId, x => x.Store, x => x.Category, x => x.ProductInMenus);
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

        public async Task<Pagination<ProductViewModel>?> GetCache(GetProductsByStoreIdQuery request)
        {

            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");

            var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
            if (cacheResult!.Count > 0)
            {
                var result = cacheResult.Where(x => x.StoreId == request.StoreId).ToList();
                if (result.Count == 0) return null;

                var cacheViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(result).ToList();
                for (int i = 0; i < cacheViewModels.Count; i++)
                {
                    cacheViewModels[i].ProductMenuId = result[i].ProductInMenus.First().Id;
                    cacheViewModels[i].QuantityInDay = result[i].ProductInMenus.First().QuantityInDay;
                    cacheViewModels[i].MenuId = result[i].ProductInMenus.First().MenuId;
                    cacheViewModels[i].QuantityUsed = result[i].ProductInMenus.First().QuantityUsed;
                    cacheViewModels[i].SalePrice = result[i].ProductInMenus.First().SalePrice;
                }
                if (request.MenuId != Guid.Empty) cacheViewModels = cacheViewModels.Where(x => x.MenuId == request.MenuId).ToList();
                var filterRe = request.Filter!.Count > 0 ? new List<ProductViewModel>() : cacheViewModels.AsEnumerable();
                if (request.Filter!.Count > 0)
                {
                    foreach (var filter in request.Filter)
                    {
                        filterRe = filterRe.Union(FilterUtilities.SelectItems(cacheViewModels, filter.Key, filter.Value));
                    }
                }

                return new Pagination<ProductViewModel>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = filterRe.Count(),
                    Items = PaginatedList<ProductViewModel>.Create(
                       source: filterRe.OrderByDescending(x => x.CreationDate).AsQueryable(),
                       pageIndex: request.PageNumber,
                       pageSize: request.PageSize
                    )
                };
            }
            return null;
        }
    }
}