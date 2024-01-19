using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Entities;

namespace PTP.Application.Features.ProductMenus.Commands;

public class CreateProductMenuCommand:IRequest<ProductMenuViewModel>
{
    public ProductMenuCreateModel CreateModel{get;set;}=default!;

    public class CommmandValidation : AbstractValidator<CreateProductMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.ActualPrice).GreaterThan(0).NotNull().NotEmpty().WithMessage("ActualPrice must not null or empty");
            RuleFor(x => x.CreateModel.ProductId).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.MenuId).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
        }
    }
    public class CommandHandler : IRequestHandler<CreateProductMenuCommand, ProductMenuViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            _logger=logger;
        }
        public async Task<ProductMenuViewModel> Handle(CreateProductMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create ProductMenu:\n");
            var productMenu= _mapper.Map<ProductInMenu>(request.CreateModel);

            await _unitOfWork.ProductInMenuRepository.AddAsync(productMenu);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductMenuViewModel>(productMenu);
        }
    }
}