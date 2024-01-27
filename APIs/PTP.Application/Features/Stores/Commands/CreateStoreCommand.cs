using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
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
                RuleFor(x => x.CreateModel.Address).NotNull().NotEmpty().WithMessage("Address must not null or empty");
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

                var isDup = await _unitOfWork.UserRepository.WhereAsync(x => x.PhoneNumber!.ToLower() == request.CreateModel.PhoneNumber!.ToLower());
                if (isDup.Count() > 0)
                    throw new Exception($"Error: {nameof(CreateStoreCommand)}_ phone is duplicate!");

                store.OpenedTime = TimeSpan.ParseExact(request.CreateModel.OpenedTime, @"hh\:mm", CultureInfo.InvariantCulture);
                store.ClosedTime = TimeSpan.ParseExact(request.CreateModel.ClosedTime, @"hh\:mm", CultureInfo.InvariantCulture);
                //Get Lat, Lng from address
                var location = await _locationService.GetGeometry(request.CreateModel.Address);
                store.Latitude = location.Lat;
                store.Longitude = location.Lng;

                //Add Image to FireBase
                var image = await request.CreateModel.File!.UploadFileAsync(FolderKey.STORE, _appSettings);
                store.ImageName = image.FileName;
                store.ImageURL = image.URL;

                //Config RelationShip
                store.WalletId = await CreateWallet(store.Id);
                store.StoreCode = (await _unitOfWork.StoreRepository.GetAllAsync()).Count + 1;
                store.UserId = await CreateUser(store);

                await _unitOfWork.StoreRepository.AddAsync(store);
                if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.STORE);
                return _mapper.Map<StoreViewModel>(store);
            }

            private async Task<Guid> CreateWallet(Guid storeId)
            {
                var wallet = new Wallet { Name = "Store-Wallet", Amount = 0, WalletType = WalletTypeEnum.Store.ToString(), StoreId = storeId };
                await _unitOfWork.WalletRepository.AddAsync(wallet);
                return wallet.Id;
            }

            private async Task<Guid> CreateUser(Store store)
            {

                UserViewModel result = await mediator.Send(new CreateUserCommand
                {
                    Model = new UserCreateModel
                    {
                        DateOfBirth = DateTime.Now,
                        Email = $"Store{store.StoreCode}@gmail.com",
                        FullName = store.Name,
                        RoleName = nameof(RoleEnum.StoreManager),
                        PhoneNumber = store.PhoneNumber,
                        Password = "@Abcaz12345",
                    }
                });
                return result.Id;
            }
        }
    }

}
