using AutoMapper;
using Azure.Core;
using Firebase.Auth;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PTP.Application.Commons;
using PTP.Application.Features.Users.Commands;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Application.ViewModels.Users;
using PTP.Domain.Entities;
using PTP.Domain.Enums;
using PTP.Domain.Globals;
using System.Globalization;
using User = PTP.Domain.Entities.User;

namespace PTP.Application.Features.Stores.Commands
{

    public class CreateStoreCommand : IRequest<StoreViewModel>
    {

        public StoreCreateModel CreateModel { get; set; } = default!;

        public class CommmandValidation : AbstractValidator<CreateStoreCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
                //Format : 0123456789, 012-345-6789, and (012)-345-6789.
                RuleFor(x => x.CreateModel.PhoneNumber).NotNull().NotEmpty()
                    .Matches(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
                    .WithMessage("PhoneNumber is not correct format!");
                RuleFor(x => x.CreateModel.OpenedTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                    .WithMessage("OpenedTime must not null or empty");
                RuleFor(x => x.CreateModel.ClosedTime).NotNull().NotEmpty().Matches(@"^\d{2}:\d{2}$")
                    .WithMessage("ClosedTime must not null or empty");
                RuleFor(x => x.CreateModel.AddressNo).NotNull().NotEmpty().WithMessage("AddressNo must not null or empty");
                RuleFor(x => x.CreateModel.Street).NotNull().NotEmpty().WithMessage("Street must not null or empty");
                RuleFor(x => x.CreateModel.Ward).NotNull().NotEmpty().WithMessage("Ward must not null or empty");
                RuleFor(x => x.CreateModel.Zone).NotNull().NotEmpty().WithMessage("Zone must not null or empty");
                RuleFor(x => x.CreateModel.Latitude).NotNull().NotEmpty().WithMessage("Latitude must not null or empty");
                RuleFor(x => x.CreateModel.Longitude).NotNull().NotEmpty().WithMessage("Longitude must not null or empty");
                RuleFor(x => x.CreateModel.File).NotNull().NotEmpty().WithMessage("File must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<CreateStoreCommand, StoreViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILocationService _locationService;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;

            private ICacheService _cacheService;
            private AppSettings _appSettings;
            private readonly IMediator mediator;

            public CommandHandler(IUnitOfWork unitOfWork,
                 IMapper mapper, ILogger<CommandHandler> logger,
                 ILocationService locationService,
                 AppSettings appSettings,
                 ICacheService cacheService,
                 IMediator mediator)
            {
                this.mediator = mediator;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _locationService = locationService;
                _appSettings = appSettings;
                _cacheService = cacheService;
            }


            public async Task<StoreViewModel> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create Store:\n");

                var store = _mapper.Map<Store>(request.CreateModel);

                #region CHECK TIME
                // Check Valid Time opentime < closetime

                store.OpenedTime = TimeSpan.ParseExact(request.CreateModel.OpenedTime, @"hh\:mm", CultureInfo.InvariantCulture);
                store.ClosedTime = TimeSpan.ParseExact(request.CreateModel.ClosedTime, @"hh\:mm", CultureInfo.InvariantCulture);

                if (store.OpenedTime >= store.ClosedTime) throw new BadRequestException("Close Time must higher than Open Time");
                #endregion

                var isDup = await _unitOfWork.UserRepository.WhereAsync(x => x.PhoneNumber!.ToLower() == request.CreateModel.PhoneNumber!.ToLower());
                if (isDup.Count() > 0)
                    throw new Exception($"Error: {nameof(CreateStoreCommand)}_ phone is duplicate!");

                store.UserId = await CreateUser(store);

                #region Get Lat, Long by Address - Now not use
                //Get Lat, Lng from address
                var addressStr = $"{request.CreateModel.AddressNo},{request.CreateModel.Street},{request.CreateModel.Ward},{request.CreateModel.Zone}";
                var location = await _locationService.GetGeometry(addressStr);
                store.Latitude = location.Lat;
                store.Longitude = location.Lng;
                #endregion

                #region Add image
                //Add Image to FireBase
                var image = await request.CreateModel.File!.UploadFileAsync(FolderKey.STORE, _appSettings);
                store.ImageName = image.FileName;
                store.ImageURL = image.URL;
                #endregion

                //Config RelationShip
                // store.WalletId=await CreateWallet(store.Id);

                if (request.CreateModel.StationIds != null)
                {
                    await AddStationsToStore(request.CreateModel.StationIds, store);
                }
                await CreateMenuDefault(store);
                await _unitOfWork.StoreRepository.AddAsync(store);
                if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.STORE);
                return _mapper.Map<StoreViewModel>(store);
            }

            private async Task AddStationsToStore(List<Guid> StationIds, Store store)
            {
                var stations = await _unitOfWork.StationRepository.WhereAsync(x => StationIds.Contains(x.Id));
                if (stations.Count == 0) throw new BadRequestException("No Station found!");
                // string errors="";
                for (int i = 0; i < stations.Count; i++)
                {
                    stations[i].StoreId = store.Id;
                    //var distance= await _locationService.GetDistance(store.Latitude,store.Longitude,stations[i].Latitude,stations[i].Longitude,"bike");
                    // if(distance >1000) errors+=$"Khoảng cách tới trạm {stations[i].Name} không quá 1000m";
                }
                // if(!errors.IsNullOrEmpty()) throw new BadRequestException(errors);
                _unitOfWork.StationRepository.UpdateRange(stations);
            }

            private async Task<Guid> CreateWallet(Guid userId)
            {
                var wallet = new Wallet { Name = "Store-Wallet", Amount = 0, WalletType = WalletTypeEnum.Store.ToString(), UserId = userId };
                await _unitOfWork.WalletRepository.AddAsync(wallet);
                return wallet.Id;
            }

            private async Task<Guid> CreateUser(Store store)
            {
                var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == nameof(RoleEnum.StoreManager).ToLower())
                ?? throw new Exception($"Error: {nameof(CreateUserCommand)}_no_role_found: role: {RoleEnum.StoreManager}");
                Random random = new Random();
                User user = new User
                {
                    FullName = store.Name,
                    PhoneNumber = store.PhoneNumber,
                    Password = "@Abcaz12345",
                    Email = $"Store{random.Next(1000)}@gmail.com",
                    StoreId = store.Id,
                    RoleId = role!.Id
                };
                user.WalletId = await CreateWallet(user.Id);

                await _unitOfWork.UserRepository.AddAsync(user);

                if (!await CreateUserToFirebaseAsync(user.Email, user.Password)) throw new Exception($"Create Account to FireBase Fail!");
                return user.Id;
            }
            private async Task CreateMenuDefault(Store store)
            {
                var menu = new Menu
                {
                    Name = "Tất cả lịch bán",
                    Description = "Menu cho tất cả lịch bán từ T2 đến CN",
                    StartTime = store.OpenedTime,
                    EndTime = store.ClosedTime,
                    // StartTime = TimeSpan.ParseExact("06:00", @"hh\:mm", CultureInfo.InvariantCulture),
                    // EndTime = TimeSpan.ParseExact("22:00", @"hh\:mm", CultureInfo.InvariantCulture),
                    DateApply = "Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday",
                    Status = nameof(DefaultStatusEnum.Active),
                    IsApplyForAll = true,
                    StoreId = store.Id
                };
                await _unitOfWork.MenuRepository.AddAsync(menu);
            }

            private async Task<bool> CreateUserToFirebaseAsync(string email, string password)
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
                try
                {
                    var result = await auth.CreateUserWithEmailAndPasswordAsync(email: email, password: password);
                    if (result.User is not null)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex}");
                }

            }
        }
    }

}
