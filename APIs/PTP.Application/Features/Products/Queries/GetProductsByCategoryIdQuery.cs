using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Queries;

public class GetProductsByCategoryIdQuery:IRequest<IEnumerable<ProductViewModel>>
{
    public Guid CategoryId { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetProductsByCategoryIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.CategoryId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }
    public class QueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductViewModel>>
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
        public async Task<IEnumerable<ProductViewModel>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
            if (cacheResult!.Count>0)
            {
                return _mapper.Map<IEnumerable<ProductViewModel>>(cacheResult.Where(x=>x.CategoryId==request.CategoryId));
            }
            var product = await _unitOfWork.ProductRepository.WhereAsync(x=>x.CategoryId==request.CategoryId,x=>x.Store,x=>x.Category);
            if (product is null) throw new BadRequestException($"Category with ID-{request.CategoryId} is not exist any products!");
            await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, product);
            return _mapper.Map<IEnumerable<ProductViewModel>>(product);
        }
    }
}