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

public class GetProductByQuery : IRequest<ProductViewModel>
{
    public Guid Id { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetProductByQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetProductByQuery, ProductViewModel>
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
        public async Task<ProductViewModel> Handle(GetProductByQuery request, CancellationToken cancellationToken)
        {
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetAsync<Product>(CacheKey.PRODUCT + request.Id);
            if (cacheResult is not null)
            {
                return _mapper.Map<ProductViewModel>(cacheResult);
            }
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id, x => x.Store, x => x.Category, x => x.ProductInMenus);
            if (product is null) throw new BadRequestException($"Product with ID-{request.Id} is not exist!");
            await _cacheService.SetAsync<Product>(CacheKey.PRODUCT + request.Id, product);
            var result = _mapper.Map<ProductViewModel>(product);
            result.ProductMenuId = product.ProductInMenus.First().Id;
            result.QuantityInDay = product.ProductInMenus.First().QuantityInDay;
            result.MenuId = product.ProductInMenus.First().MenuId;

            return result;
        }
    }
}