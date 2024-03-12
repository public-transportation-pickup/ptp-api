using FluentValidation;
using MediatR;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public class CommmandValidation : AbstractValidator<DeleteProductCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

        }
    }

    public class CommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public CommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveAsync(CacheKey.PRODUCT + request.Id);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (product is null) throw new NotFoundException($"Product with Id-{request.Id} is not exist!");
            _unitOfWork.ProductRepository.SoftRemove(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}