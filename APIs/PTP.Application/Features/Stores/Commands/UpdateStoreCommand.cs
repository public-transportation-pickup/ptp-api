using System.Reflection.Metadata;
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
using Microsoft.IdentityModel.Tokens;

namespace PTP.Application.Features.Stores.Commands;
public class UpdateStoreCommand : IRequest<bool>
{
    public StoreUpdateModel StoreUpdate { get; set; } = default!;

    public class CommmandValidation : AbstractValidator<UpdateStoreCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.StoreUpdate.Id).NotNull().NotEmpty().WithMessage("Name must not null or empty");
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
            _locationService = locationService;
            _appSettings = appSettings;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache

            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveByPrefixAsync<Store>(CacheKey.STORE);

            var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.StoreUpdate.Id);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(store!.UserId);
            if (user is not null)
            {
                user.Email = request.StoreUpdate.Email;
                user.FullName = request.StoreUpdate.ManagerName;
                user.PhoneNumber = request.StoreUpdate.ManagerPhone;
                user.DateOfBirth = request.StoreUpdate.DateOfBirth;
            }
            if (store is null) throw new NotFoundException($"Store with Id-{request.StoreUpdate.Id} is not exist!");
            store = _mapper.Map(request.StoreUpdate, store);

            #region CHECCK TIME
            // Check Valid Time opentime < closetime

            store.OpenedTime = TimeSpan.ParseExact(request.StoreUpdate.OpenedTime, @"hh\:mm", CultureInfo.InvariantCulture);
            store.ClosedTime = TimeSpan.ParseExact(request.StoreUpdate.ClosedTime, @"hh\:mm", CultureInfo.InvariantCulture);

            if (store.OpenedTime >= store.ClosedTime) throw new BadRequestException("Close Time must higher than Open Time");
            #endregion
            await CheckDistance(request.StoreUpdate);

            if (request.StoreUpdate.File is not null)
            {
                await store.ImageName.RemoveFileAsync(FolderKey.STORE, appSettings: _appSettings);
                var image = await request.StoreUpdate.File!.UploadFileAsync(FolderKey.STORE, _appSettings);
                store.ImageName = image.FileName;
                store.ImageURL = image.URL;
            }
            if (user is not null)
                _unitOfWork.UserRepository.Update(user);
            _unitOfWork.StoreRepository.Update(store);
            await UpdateStation(request.StoreUpdate);
            return await _unitOfWork.SaveChangesAsync();
        }
        private async Task UpdateStation(StoreUpdateModel model)
        {
            if (model!.StationIds == null) return;
            var rootStations = await _unitOfWork.StationRepository.WhereAsync(x => x.StoreId == model.Id);
            var modelStations = await _unitOfWork.StationRepository.WhereAsync(x => model.StationIds.Contains(x.Id));
            //Delete station
            var deleteStations = rootStations.Where(x => !modelStations.Any(y => y.Id == x.Id)).ToList();
            if (deleteStations.Count > 0)
            {
                for (int i = 0; i < deleteStations.Count; i++)
                {
                    deleteStations[i].StoreId = null;
                }
            }

            //Add staions
            var newStations = modelStations.Where(x => !rootStations.Any(y => y.Id == x.Id)).ToList();
            if (newStations.Count > 0)
            {
                for (int i = 0; i < newStations.Count; i++)
                {
                    newStations[i].StoreId = model.Id;
                }
            }

            //Update
            _unitOfWork.StationRepository.UpdateRange(newStations.Concat(deleteStations).ToList());

        }

        private async Task CheckDistance(StoreUpdateModel store)
        {
            if (store!.StationIds == null) return;
            var stations = await _unitOfWork.StationRepository.WhereAsync(x => store.StationIds.Contains(x.Id));
            string errors = "";
            for (int i = 0; i < stations.Count; i++)
            {
                var distance = await _locationService.GetDistance(store.Latitude, store.Longitude, stations[i].Latitude, stations[i].Longitude, "bike");
                if (distance > 1000) errors += $"Khoảng cách tới trạm {stations[i].Name} không quá 1000m";
            }
            if (!errors.IsNullOrEmpty()) throw new BadRequestException(errors);
        }
    }

}