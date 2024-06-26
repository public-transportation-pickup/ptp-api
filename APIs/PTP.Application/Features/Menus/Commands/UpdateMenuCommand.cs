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

public class UpdateMenuCommand : IRequest<bool>
{
    public MenuUpdateModel UpdateModel { get; set; } = default!;
    public class CommmandValidation : AbstractValidator<UpdateMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.UpdateModel.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.UpdateModel.StartTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                .WithMessage("StartTime must not null or empty");
            RuleFor(x => x.UpdateModel.EndTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                .WithMessage("EndTime must not null or empty");
            RuleFor(x => x.UpdateModel.DateApply).NotNull().NotEmpty().WithMessage("DateFilter must not null or empty");
            RuleFor(x => x.UpdateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            RuleFor(x => x.UpdateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<UpdateMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        public CommandHandler(IUnitOfWork unitOfWork,
                ICacheService cacheService,
                ILogger<CommandHandler> logger,
                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update Menu:\n");
            TimeSpan.TryParseExact(request.UpdateModel.StartTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime);
            TimeSpan.TryParseExact(request.UpdateModel.EndTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime);
            // if (startTime >= endTime) throw new BadRequestException("Start Time must higher than End Time");

            //Remove From Cache       

            var menu = await _unitOfWork.MenuRepository.GetByIdAsync(request.UpdateModel.Id);
            if (menu is null) throw new NotFoundException($"Menu with Id-{request.UpdateModel.Id} is not exist!");

            // if (menu.StartTime != startTime || menu.EndTime != endTime || menu.DateApply != request.UpdateModel.DateApply) await CheckTime(menu.StoreId, menu.DateApply, startTime, endTime);
            menu = _mapper.Map(request.UpdateModel, menu);

            _unitOfWork.MenuRepository.Update(menu);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result)
            {
                if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                await _cacheService.RemoveByPrefixAsync<Menu>(CacheKey.MENU);
            }
            return result;
        }

        // private async Task<bool> CheckTime(Guid storeId, DateTime dateApply, TimeSpan sTime, TimeSpan eTime)
        // {
        //     var menus = await _unitOfWork.MenuRepository.WhereAsync(x =>
        //                          x.StoreId == storeId);

        //     if (menus.Count == 0) return true;

        //     foreach (var item in menus)
        //     {
        //         if (item.StartTime <= sTime && item.EndTime > sTime) throw new BadRequestException($"Start time is duplicate with menu-{item.Id}");
        //         if (item.StartTime < eTime && item.EndTime >= eTime) throw new BadRequestException($"End time is duplicate with menu-{item.Id}");
        //     }
        //     return true;
        // }
    }
}