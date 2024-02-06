using AutoMapper;
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

public class GetAllProductQuery:IRequest<PaginatedList<ProductViewModel >>
{
        public Dictionary<string, string>? Filter { get; set; } = default!;
        public int PageNumber{get;set;}
        public int PageSize{get;set;}
    public class QueryHandler : IRequestHandler<GetAllProductQuery, PaginatedList<ProductViewModel>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService  _cacheService;
        private ILogger<QueryHandler> logger;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            this.logger = logger;
        }

        public async Task<PaginatedList<ProductViewModel>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");

            var cacheResult= await GetCache(request);
            if(cacheResult is not null) return cacheResult;
            
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x=>x.Store,x=>x.Category);
            if (products.Count == 0) throw new NotFoundException("There are no product in DB!");
            await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, products);
            var viewModels= _mapper.Map<IEnumerable<ProductViewModel>>(products);
            
            var filterResult = request.Filter.Count > 0 ? new List<ProductViewModel>() : viewModels;

                if(request.Filter!.Count>0)
                {
                    foreach(var filter in request.Filter) 
                    {
                        filterResult=filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                    }
                }
                
                return PaginatedList<ProductViewModel>.Create(
                            source:filterResult.AsQueryable(),
                            pageIndex:request.PageNumber,
                            pageSize:request.PageSize
                    );
        }

        public async Task<PaginatedList<ProductViewModel>?> GetCache(GetAllProductQuery request)
            {
                
                if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");

                var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
                if (cacheResult!.Count > 0)
                {
                    var cacheViewModels= _mapper.Map<IEnumerable<ProductViewModel>>(cacheResult);   
                    var filterRe= request.Filter!.Count > 0 ? new List<ProductViewModel>(): cacheViewModels;
                    if(request.Filter!.Count>0)
                    {
                        foreach(var filter in request.Filter) 
                        {
                            filterRe=filterRe.Union(FilterUtilities.SelectItems(cacheViewModels, filter.Key, filter.Value));
                        }
                    }               
                    return PaginatedList<ProductViewModel>.Create(
                            source: filterRe.AsQueryable(),
                            pageIndex:request.PageNumber,
                            pageSize:request.PageSize
                    );
                }
                return null;
            }
    }
}