using System.Globalization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Categories.Commands;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public class CommmandValidation : AbstractValidator<DeleteCategoryCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

        }
    }
    public class CommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveAsync(CacheKey.CATE + request.Id);

            var cate = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            if (cate is null) throw new NotFoundException($"Category with Id-{request.Id} is not exist!");
            _unitOfWork.CategoryRepository.SoftRemove(cate);
            return await _unitOfWork.SaveChangesAsync();
        }
    }

}
