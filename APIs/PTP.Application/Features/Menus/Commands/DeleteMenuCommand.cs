using FluentValidation;
using MediatR;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Menus.Commands;

public class DeleteMenuCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public class CommmandValidation : AbstractValidator<DeleteMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

        }
    }
    public class CommandHandler : IRequestHandler<DeleteMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveAsync(CacheKey.MENU + request.Id);

            var menu = await _unitOfWork.MenuRepository.GetByIdAsync(request.Id);
            if (menu is null) throw new NotFoundException($"Menu with Id-{request.Id} is not exist!");
            _unitOfWork.MenuRepository.SoftRemove(menu);
            return await _unitOfWork.SaveChangesAsync();
        }
    }

}
