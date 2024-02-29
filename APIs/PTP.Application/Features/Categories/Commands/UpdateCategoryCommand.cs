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
using PTP.Domain.Globals;

namespace PTP.Application.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<bool>
{
    public CategoryUpdateModel UpdateModel { get; set; } = default!;
    public class CommmandValidation : AbstractValidator<UpdateCategoryCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.UpdateModel.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.UpdateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private AppSettings _appSettings;
        public CommandHandler(IUnitOfWork unitOfWork,
                ICacheService cacheService,
                ILogger<CommandHandler> logger,
                IMapper mapper,
                AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update Menu:\n");
            //Remove From Cache       

            var cate = await _unitOfWork.CategoryRepository.GetByIdAsync(request.UpdateModel.Id);
            if (cate is null) throw new NotFoundException($"Category with Id-{request.UpdateModel.Id} is not exist!");

            cate = _mapper.Map(request.UpdateModel, cate);
            if (request.UpdateModel.Image is not null)
            {
                await cate.ImageName.RemoveFileAsync(FolderKey.CATEGORY, appSettings: _appSettings);
                var image = await request.UpdateModel.Image!.UploadFileAsync(FolderKey.CATEGORY, _appSettings);
                cate.ImageName = image.FileName;
                cate.ImageURL = image.URL;
            }

            _unitOfWork.CategoryRepository.Update(cate);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result)
            {
                if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.CATE);
            }
            return result;
        }
    }
}