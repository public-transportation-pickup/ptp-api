using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Queries;

public class GetAllProductQuery:IRequest<IEnumerable<ProductViewModel >>
{
    public class QueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<ProductViewModel>>
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

        public async Task<IEnumerable<ProductViewModel>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
            if (cacheResult!.Count > 0)
            {
                return _mapper.Map<IEnumerable<ProductViewModel>>(cacheResult);
            }
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x=>x.Store,x=>x.Category);
            if (products.Count == 0) throw new NotFoundException("There are no product in DB!");
            await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, products);
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }
    }
}