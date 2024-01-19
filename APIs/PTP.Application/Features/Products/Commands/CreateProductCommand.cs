using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Commands;

public class CreateProductCommand:IRequest<ProductViewModel>
{
    public ProductCreateModel CreateModel{get;set;}=default!;

    public class CommmandValidation : AbstractValidator<CreateProductCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.Price).GreaterThan(0).NotNull().NotEmpty().WithMessage("Price must not null or empty");
            RuleFor(x => x.CreateModel.PreparationTime).GreaterThan(0).NotNull().NotEmpty().WithMessage("PreparationTime must not null or empty");
            RuleFor(x => x.CreateModel.CategoryId).NotNull().NotEmpty().WithMessage("CategoryId must not null or empty");
            RuleFor(x => x.CreateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
            RuleFor(x => x.CreateModel.Image).NotNull().NotEmpty().WithMessage("Image must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private AppSettings _appSettings;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            _logger=logger;
            _appSettings=appSettings;
        }
        public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Product:\n");
            var product= _mapper.Map<Product>(request.CreateModel);

            //Add Image to FireBase
            var image = await request.CreateModel.Image!.UploadFileAsync(FolderKey.PRODUCT,_appSettings);
            product.ImageName=image.FileName;
            product.ImageURL=image.URL;
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductViewModel>(product);
        }
    }
}
