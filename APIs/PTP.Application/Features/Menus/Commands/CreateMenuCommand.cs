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

public class CreateMenuCommand : IRequest<List<MenuViewModel>>
{
    public MenuCreateModel CreateModel { get; set; } = default!;

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
            RuleFor(x => x.CreateModel.DatesApply).NotNull().NotEmpty().WithMessage("Date Apply must not null or empty");
            RuleFor(x => x.CreateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            RuleFor(x => x.CreateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateMenuCommand, List<MenuViewModel>>
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
        public async Task<List<MenuViewModel>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Menu:\n");

            var listMenu = new List<Menu>();
            for (int i = 0; i < request.CreateModel.DatesApply.Count; i++)
            {
                var menu = _mapper.Map<Menu>(request.CreateModel);
                menu.StartTime = TimeSpan.ParseExact(request.CreateModel.StartTime, @"hh\:mm", CultureInfo.InvariantCulture);
                menu.EndTime = TimeSpan.ParseExact(request.CreateModel.EndTime, @"hh\:mm", CultureInfo.InvariantCulture);
                if (menu.StartTime >= menu.EndTime) throw new BadRequestException("Start Time must higher than End Time");
                menu.DateApply = request.CreateModel.DatesApply[i];
                if (!await CheckTime(menu)) throw new BadRequestException("Time is not valid!");
                listMenu.Add(menu);
            }
            await _unitOfWork.MenuRepository.AddRangeAsync(listMenu);
            if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
            await _cacheService.RemoveByPrefixAsync(CacheKey.MENU);
            return _mapper.Map<List<MenuViewModel>>(listMenu);
        }

        private async Task<bool> CheckTime(Menu menu)
        {
            var menus = await _unitOfWork.MenuRepository.WhereAsync(x =>
                                 x.StoreId == menu.StoreId && x.DateApply == menu.DateApply);

            if (menus.Count == 0) return true;

            foreach (var item in menus)
            {
                if (item.StartTime <= menu.StartTime && item.EndTime > menu.StartTime) throw new BadRequestException($"Start time is duplicate with menu-{item.Id} in {item.DateApply} ");
                if (item.StartTime < menu.EndTime && item.EndTime >= menu.EndTime) throw new BadRequestException($"End time is duplicate with menu-{item.Id}  in {item.DateApply}");
            }
            return true;
        }
    }
}