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

            var cacheResult = await GetCache(request);
            if (cacheResult is not null) return cacheResult;

            var product = await _unitOfWork.ProductRepository.WhereAsync(x => x.StoreId == request.StoreId, x => x.Store, x => x.Category);
            if (product is null) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any products!");
            await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, product);
            //    var result= _mapper.Map<IEnumerable<ProductViewModel>>(product);
            //    var viewModels= request.CategoryId==Guid.Empty? result: result.Where(x=>x.CategoryId==request.CategoryId);
            var viewModels = _mapper.Map<IEnumerable<ProductViewModel>>(product);

            var filterResult = request.Filter.Count > 0 ? new List<ProductViewModel>() : viewModels;

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

            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");

            var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
            if (cacheResult!.Count > 0)
            {
                var result = cacheResult.Where(x => x.StoreId == request.StoreId);
                if (result == null) return null;

                var cacheViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(result);
                var filterRe = request.Filter!.Count > 0 ? new List<ProductViewModel>() : cacheViewModels;
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