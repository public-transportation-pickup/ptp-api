using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Commands;

public class UpdateProductCommand : IRequest<bool>
{
    public ProductUpdateModel UpdateModel { get; set; } = default!;

    public class CommmandValidation : AbstractValidator<UpdateProductCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.UpdateModel.Price).GreaterThan(0).NotNull().NotEmpty().WithMessage("Price must not null or empty");
            RuleFor(x => x.UpdateModel.CategoryId).NotNull().NotEmpty().WithMessage("CategoryId must not null or empty");
            RuleFor(x => x.UpdateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
            RuleFor(x => x.UpdateModel.MenuId).NotNull().NotEmpty().WithMessage("MenuId must not null or empty");
            RuleFor(x => x.UpdateModel.QuantityInDay).NotNull().NotEmpty().WithMessage("QuantityInDay must not null or empty");
            RuleFor(x => x.UpdateModel.NumProcessParallel).NotNull().NotEmpty().WithMessage("NumProcessParallel must not null or empty");
            RuleFor(x => x.UpdateModel.PreparationTime).NotNull().NotEmpty().WithMessage("NumProcessParallel must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private AppSettings _appSettings;

        private readonly ICacheService _cacheService;

        public CommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper, ILogger<CommandHandler> logger,
            AppSettings appSettings,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _appSettings = appSettings;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveByPrefixAsync<Product>(CacheKey.PRODUCT);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.UpdateModel.Id);
            if (product is null) throw new NotFoundException($"product with Id-{request.UpdateModel.Id} is not exist!");
            product = _mapper.Map(request.UpdateModel, product);

            if (request.UpdateModel.Image is not null)
            {
                await product.ImageName.RemoveFileAsync(FolderKey.PRODUCT, appSettings: _appSettings);
                var image = await request.UpdateModel.Image!.UploadFileAsync(FolderKey.PRODUCT, _appSettings);
                product.ImageName = image.FileName;
                product.ImageURL = image.URL;
            }

            await UpdateProductMenu(request.UpdateModel);
            _unitOfWork.ProductRepository.Update(product);
            return await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdateProductMenu(ProductUpdateModel model)
        {
            var productMenu = await _unitOfWork.ProductInMenuRepository.GetByIdAsync(model.ProductMenuId);
            if (productMenu == null) throw new BadRequestException("Not Found Product in Menu!");
            productMenu.Status = model.Status;
            productMenu.MenuId = model.MenuId;
            productMenu.SalePrice = model.SalePrice;
            productMenu.QuantityInDay = model.QuantityInDay;
            productMenu.NumProcessParallel = model.NumProcessParallel;
            productMenu.PreparationTime = model.PreparationTime;

            _unitOfWork.ProductInMenuRepository.Update(productMenu);
        }

    }
}