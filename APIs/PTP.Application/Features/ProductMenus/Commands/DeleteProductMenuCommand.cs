using FluentValidation;
using MediatR;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Globals;

namespace PTP.Application.Features.ProductMenus.Commands;

public class DeleteProductMenuCommand:IRequest<bool>
{
    public Guid Id{get;set;}
    public class CommmandValidation : AbstractValidator<DeleteProductMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            
        }
    }

    public class CommandHandler : IRequestHandler<DeleteProductMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService=cacheService;
        }

        public async Task<bool> Handle(DeleteProductMenuCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
           if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
           await _cacheService.RemoveAsync(CacheKey.PRODUCTMENU+request.Id);
            
            var productMenu = await _unitOfWork.ProductInMenuRepository.GetByIdAsync(request.Id);
            if(productMenu is null ) throw new NotFoundException($"ProductMenu with Id-{request.Id} is not exist!");
            _unitOfWork.ProductInMenuRepository.SoftRemove(productMenu);
            return await _unitOfWork.SaveChangesAsync();
        }
    }

}