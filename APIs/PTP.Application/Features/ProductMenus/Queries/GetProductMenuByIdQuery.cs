using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.ProductMenus.Queries;

public class GetProductMenuByIdQuery : IRequest<ProductMenuViewModel>
{
    public Guid Id { get; set; }
    public class CommmandValidation : AbstractValidator<GetProductMenuByIdQuery>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }

    }

    public class QueryHandler : IRequestHandler<GetProductMenuByIdQuery, ProductMenuViewModel>
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
        public async Task<ProductMenuViewModel> Handle(GetProductMenuByIdQuery request, CancellationToken cancellationToken)
        {
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetAsync<ProductInMenu>(CacheKey.PRODUCTMENU + request.Id);
            if (cacheResult is not null)
            {
                return _mapper.Map<ProductMenuViewModel>(cacheResult);
            }
            var productMenu = await _unitOfWork.ProductInMenuRepository
                                .FirstOrDefaultAsync(x => x.Id == request.Id,
                                            x => x.Menu,
                                            x => x.Product,
                                            x => x.Product.Category);
            if (productMenu is null) throw new BadRequestException($"productMenu with ID-{request.Id} is not exist!");
            await _cacheService.SetAsync<ProductInMenu>(CacheKey.PRODUCTMENU + request.Id, productMenu);
            return _mapper.Map<ProductMenuViewModel>(productMenu);
        }
    }
}