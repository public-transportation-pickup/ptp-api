using System.Globalization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.ViewModels.Categories;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Categories.Commands;
public class CreateCategoryCommand:IRequest<CategoryViewModel>
{
    public CategoryCreateModel CreateModel{get;set;}=default!;

    public class CommmandValidation : AbstractValidator<CreateCategoryCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
            RuleFor(x => x.CreateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            _logger=logger;
        }
        public async Task<CategoryViewModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Cate:\n");
            var cate= _mapper.Map<Category>(request.CreateModel);

            await _unitOfWork.CategoryRepository.AddAsync(cate);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryViewModel>(cate);
        }
    }
}