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
            TimeSpan.TryParseExact(request.CreateModel.StartTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime);
            TimeSpan.TryParseExact(request.CreateModel.EndTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime);
            if (startTime>=endTime) throw new BadRequestException("Start Time must higher than End Time");

            if(!await CheckTime(request.CreateModel.StoreId,startTime,endTime)) throw new BadRequestException("Time is not valid!");
            var menu= _mapper.Map<Menu>(request.CreateModel);
            

            await _unitOfWork.MenuRepository.AddAsync(menu);
            if( !await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.MENU);
            return _mapper.Map<MenuViewModel>(menu);
        }

        private async Task<bool> CheckTime(Guid storeId,TimeSpan sTime, TimeSpan eTime) 
        {
            var menus = await _unitOfWork.MenuRepository.WhereAsync(x =>
                                 x.StoreId == storeId);
            
            if(menus.Count==0) return true;

            foreach (var item in menus)
            {
                TimeSpan.TryParseExact(item.StartTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime);
                TimeSpan.TryParseExact(item.EndTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime);
                if (startTime <= sTime && endTime > sTime) throw new BadRequestException($"Start time is duplicate with menu-{item.Id}" );
                if (startTime < eTime && endTime >= eTime) throw new BadRequestException($"End time is duplicate with menu-{item.Id}" );
            }
            return true;
        }
    }
}