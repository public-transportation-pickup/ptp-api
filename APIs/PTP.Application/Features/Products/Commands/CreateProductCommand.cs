using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Enums;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<ProductViewModel>
{
    public ProductCreateModel CreateModel { get; set; } = default!;

    public class CommmandValidation : AbstractValidator<CreateProductCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.Price).GreaterThan(0).NotNull().NotEmpty().WithMessage("Price must not null or empty");
            RuleFor(x => x.CreateModel.CategoryId).NotNull().NotEmpty().WithMessage("CategoryId must not null or empty");
            RuleFor(x => x.CreateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
            RuleFor(x => x.CreateModel.Image).NotNull().NotEmpty().WithMessage("Image must not null or empty");
            RuleFor(x => x.CreateModel.MenuId).NotNull().NotEmpty().WithMessage("MenuId must not null or empty");
            RuleFor(x => x.CreateModel.QuantityInDay).NotNull().NotEmpty().WithMessage("QuantityInDay must not null or empty");
            RuleFor(x => x.CreateModel.NumProcessParallel).NotNull().NotEmpty().WithMessage("NumProcessParallel must not null or empty");
            RuleFor(x => x.CreateModel.PreparationTime).NotNull().NotEmpty().WithMessage("NumProcessParallel must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private AppSettings _appSettings;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                AppSettings appSettings,
                ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _appSettings = appSettings;
            _cacheService = cacheService;
        }
        public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Product:\n");
            var product = _mapper.Map<Product>(request.CreateModel);

            //Add Image to FireBase
            var image = await request.CreateModel.Image!.UploadFileAsync(FolderKey.PRODUCT, _appSettings);
            product.ImageName = image.FileName;
            product.ImageURL = image.URL;
            await _unitOfWork.ProductRepository.AddAsync(product);
            await CreateProductMenu(product.Id, request.CreateModel);
            if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
            await _cacheService.RemoveByPrefixAsync<Product>(CacheKey.PRODUCT);
            return _mapper.Map<ProductViewModel>(product);
        }

        private async Task CreateProductMenu(Guid productId, ProductCreateModel model)
        {
            var productMenu = new ProductInMenu
            {
                MenuId = model.MenuId,
                ProductId = productId,
                PreparationTime = model.PreparationTime,
                SalePrice = model.Price,
                Status = DefaultStatusEnum.Active.ToString(),
                QuantityInDay = model.QuantityInDay,
                NumProcessParallel = model.NumProcessParallel
            };
            await _unitOfWork.ProductInMenuRepository.AddAsync(productMenu);
        }

    }
}
