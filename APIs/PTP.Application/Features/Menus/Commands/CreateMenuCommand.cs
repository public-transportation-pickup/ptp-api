using System.Globalization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

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
            RuleFor(x => x.CreateModel.StartTime).NotNull().NotEmpty().Matches(@"^(0[0-9]|1[0-2]):[0-5][0-9]\s*(AM|PM)$")
                .WithMessage("StartTime must not null or empty");
            RuleFor(x => x.CreateModel.EndTime).NotNull().NotEmpty().Matches(@"^(0[0-9]|1[0-2]):[0-5][0-9]\s*(AM|PM)$")
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
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            _logger=logger;
            _cacheService=cacheService;
        }
        public async Task<MenuViewModel> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Menu:\n");
            DateTime.TryParseExact(request.CreateModel.StartTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime);
            DateTime.TryParseExact(request.CreateModel.EndTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endTime);
            if(startTime>=endTime) throw new BadRequestException("Open Time must higher than CloseTime");
            var menu= _mapper.Map<Menu>(request.CreateModel);
           

            await _unitOfWork.MenuRepository.AddAsync(menu);
            if( !await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.MENU);
            return _mapper.Map<MenuViewModel>(menu);
        }
    }
}