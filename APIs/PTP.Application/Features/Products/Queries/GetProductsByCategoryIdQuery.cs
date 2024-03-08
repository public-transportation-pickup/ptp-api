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

public class GetProductsByCategoryIdQuery : IRequest<PaginatedList<ProductViewModel>>
{
    public Guid CategoryId { get; set; } = default!;
    public Dictionary<string, string>? Filter { get; set; } = default!;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public class QueryValidation : AbstractValidator<GetProductsByCategoryIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.CategoryId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }
    public class QueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, PaginatedList<ProductViewModel>>
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
        public async Task<PaginatedList<ProductViewModel>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");

            var cacheResult = await GetCache(request);
            if (cacheResult is not null) return cacheResult;

            var product = await _unitOfWork.ProductRepository.WhereAsync(x => x.CategoryId == request.CategoryId, x => x.Store, x => x.Category);
            if (product is null) throw new BadRequestException($"Category with ID-{request.CategoryId} is not exist any products!");

            await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, product);

            var viewModels = _mapper.Map<IEnumerable<ProductViewModel>>(product);

            var filterResult = request.Filter.Count > 0 ? new List<ProductViewModel>() : viewModels;

            if (request.Filter!.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResult = filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                }
            }

            return PaginatedList<ProductViewModel>.Create(
                        source: filterResult.AsQueryable(),
                        pageIndex: request.PageNumber,
                        pageSize: request.PageSize
                );
        }

        public async Task<PaginatedList<ProductViewModel>?> GetCache(GetProductsByCategoryIdQuery request)
        {

            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");

            var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
            if (cacheResult!.Count > 0)
            {
                var result = cacheResult.Where(x => x.CategoryId == request.CategoryId);
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
                return PaginatedList<ProductViewModel>.Create(
                        source: filterRe.AsQueryable(),
                        pageIndex: request.PageNumber,
                        pageSize: request.PageSize
                );
            }
            return null;
        }
    }
}