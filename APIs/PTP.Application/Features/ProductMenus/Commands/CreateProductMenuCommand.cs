using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.ProductMenus.Commands;

public class CreateProductMenuCommand : IRequest<ProductMenuViewModel>
{
    public ProductMenuCreateModel CreateModel { get; set; } = default!;

    public class CommmandValidation : AbstractValidator<CreateProductMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.SalePrice).GreaterThan(0).NotNull().NotEmpty().WithMessage("ActualPrice must not null or empty");
            RuleFor(x => x.CreateModel.ProductId).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.MenuId).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            RuleFor(x => x.CreateModel.PreparationTime).GreaterThan(0).NotNull().NotEmpty().WithMessage("PreparationTime must not null or empty");
            RuleFor(x => x.CreateModel.NumProcessParallel).GreaterThan(0).NotNull().NotEmpty().WithMessage("NumProcessParallel must not null or empty");
            RuleFor(x => x.CreateModel.QuantityInDay).NotNull().NotEmpty().WithMessage("Status must not null or empty");
        }
    }
    public class CommandHandler : IRequestHandler<CreateProductMenuCommand, ProductMenuViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<ProductMenuViewModel> Handle(CreateProductMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create ProductMenu:\n");
            var productMenu = _mapper.Map<ProductInMenu>(request.CreateModel);

            await _unitOfWork.ProductInMenuRepository.AddAsync(productMenu);
            if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
            await _cacheService.RemoveByPrefixAsync(CacheKey.PRODUCTMENU);
            return _mapper.Map<ProductMenuViewModel>(productMenu);
        }
    }
}