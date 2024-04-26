using System.Globalization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Categories;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Categories.Commands;
public class CreateCategoryCommand : IRequest<CategoryViewModel>
{
    public CategoryCreateModel CreateModel { get; set; } = default!;

    public class CommmandValidation : AbstractValidator<CreateCategoryCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            RuleFor(x => x.CreateModel.Image).NotNull().NotEmpty().WithMessage("Image must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private readonly ICacheService _cacheService;
        private AppSettings _appSettings;

        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                ICacheService cacheService,
                AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
            _appSettings = appSettings;
        }
        public async Task<CategoryViewModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Cate:\n");
            var cate = _mapper.Map<Category>(request.CreateModel);
            //Add Image to FireBase
            var image = await request.CreateModel.Image!.UploadFileAsync(FolderKey.CATEGORY, _appSettings);
            cate.ImageName = image.FileName;
            cate.ImageURL = image.URL;
            await _unitOfWork.CategoryRepository.AddAsync(cate);
            if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("Save changes Fail!");
            await _cacheService.RemoveByPrefixAsync<Category>(CacheKey.CATE);
            return _mapper.Map<CategoryViewModel>(cate);
        }
    }
}