using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;
using PTP.Domain.Globals;
using System.Globalization;

namespace PTP.Application.Features.Stores.Commands;
public class UpdateStoreCommand:IRequest<bool>
{
    public StoreUpdateModel StoreUpdate{get;set;}=default!;

    public class CommmandValidation : AbstractValidator<UpdateStoreCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x=>x.StoreUpdate.Id).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.StoreUpdate.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.StoreUpdate.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            //Format : 0123456789, 012-345-6789, and (012)-345-6789.
            RuleFor(x => x.StoreUpdate.PhoneNumber).NotNull().NotEmpty()
                .Matches(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
                .WithMessage("PhoneNumber is not correct format!");
            RuleFor(x => x.StoreUpdate.OpenedTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                .WithMessage("OpenedTime must not null or empty");
            RuleFor(x => x.StoreUpdate.ClosedTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                .WithMessage("ClosedTime must not null or empty");
            RuleFor(x => x.StoreUpdate.AddressNo).NotNull().NotEmpty().WithMessage("AddressNo must not null or empty");
            RuleFor(x => x.StoreUpdate.Street).NotNull().NotEmpty().WithMessage("Street must not null or empty");
            RuleFor(x => x.StoreUpdate.Ward).NotNull().NotEmpty().WithMessage("Ward must not null or empty");
            RuleFor(x => x.StoreUpdate.Zone).NotNull().NotEmpty().WithMessage("Zone must not null or empty");
            RuleFor(x => x.StoreUpdate.Latitude).NotNull().NotEmpty().WithMessage("Latitude must not null or empty");
            RuleFor(x => x.StoreUpdate.Longitude).NotNull().NotEmpty().WithMessage("Longitude must not null or empty");
            RuleFor(x => x.StoreUpdate.ActivationDate).NotNull().NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now.AddDays(3))
                .LessThanOrEqualTo(DateTime.Now.AddYears(2))
                .WithMessage("ActivationDate must not null or empty");
        }
    }
    public class CommandHandler : IRequestHandler<UpdateStoreCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private AppSettings _appSettings;

        private readonly ICacheService _cacheService;

        public CommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper, ILogger<CommandHandler> logger,
            ILocationService locationService,
            AppSettings appSettings,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _locationService=locationService;
            _appSettings=appSettings;
            _cacheService=cacheService;
        }

        public async Task<bool> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
           
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveByPrefixAsync(CacheKey.STORE);

            var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.StoreUpdate.Id);
            if(store is null ) throw new NotFoundException($"Store with Id-{request.StoreUpdate.Id} is not exist!");
            store = _mapper.Map(request.StoreUpdate,store);

            #region CHECCK TIME
            // Check Valid Time opentime < closetime

            store.OpenedTime=TimeSpan.ParseExact(request.StoreUpdate.OpenedTime, @"hh\:mm", CultureInfo.InvariantCulture);
            store.ClosedTime= TimeSpan.ParseExact(request.StoreUpdate.ClosedTime, @"hh\:mm", CultureInfo.InvariantCulture);

            if(store.OpenedTime>=store.ClosedTime) throw new BadRequestException("Close Time must higher than Open Time");
            #endregion

            
            if (request.StoreUpdate.File is not null){
                await store.ImageName.RemoveFileAsync(FolderKey.STORE, appSettings: _appSettings);
                var image = await request.StoreUpdate.File!.UploadFileAsync(FolderKey.STORE,_appSettings);
                store.ImageName=image.FileName;
                store.ImageURL=image.URL;
            }
            #region Get Lat, Long by Address - Now not use

            if(!request.StoreUpdate.AddressNo.Equals(store.AddressNo)|| !request.StoreUpdate.Street.Equals(store.Street) ||
                    !request.StoreUpdate.Zone.Equals(store.Zone)|| !request.StoreUpdate.Ward.Equals(store.Ward))
            {
                var addressStr = $"{request.StoreUpdate.AddressNo},{request.StoreUpdate.Street},{request.StoreUpdate.Ward},{request.StoreUpdate.Zone}";
                var location = await _locationService.GetGeometry(addressStr);
                store.Latitude=location.Lat;
                store.Longitude=location.Lng;
            }
            #endregion
            
            _unitOfWork.StoreRepository.Update(store);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
    
}