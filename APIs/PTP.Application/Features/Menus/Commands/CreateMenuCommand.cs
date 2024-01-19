using System.Globalization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Menus.Commands;

public class CreateMenuCommand:IRequest<MenuViewModel>
{
    public MenuCreateModel CreateModel{get;set;}=default!;

    public class CommmandValidation : AbstractValidator<CreateMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.StartTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                .WithMessage("StartTime must not null or empty");
            RuleFor(x => x.CreateModel.EndTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                .WithMessage("EndTime must not null or empty");
            RuleFor(x => x.CreateModel.DateFilter).NotNull().NotEmpty().WithMessage("DateFilter must not null or empty");
            RuleFor(x => x.CreateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            RuleFor(x => x.CreateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateMenuCommand, MenuViewModel>
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
        public async Task<MenuViewModel> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Menu:\n");
            var menu= _mapper.Map<Menu>(request.CreateModel);
            menu.StartTime=TimeSpan.ParseExact(request.CreateModel.StartTime, @"hh\:mm", CultureInfo.InvariantCulture);
            menu.EndTime=TimeSpan.ParseExact(request.CreateModel.EndTime, @"hh\:mm", CultureInfo.InvariantCulture);

            await _unitOfWork.MenuRepository.AddAsync(menu);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MenuViewModel>(menu);
        }
    }
}