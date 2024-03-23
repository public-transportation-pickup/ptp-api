using MediatR;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.MongoDbs.Carts;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.Features.Carts.Commands;
public class CreateCartCommand : IRequest<CartViewModel?>
{
    public CartCreateModel model { get; set; } = new();
    public class CommandHandler : IRequestHandler<CreateCartCommand, CartViewModel?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICartRepository cartRepository;
        private readonly IClaimsService claimsService;
        public CommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepository,
            IClaimsService claimsService)
        {
            this.unitOfWork = unitOfWork;
            this.cartRepository = cartRepository;
            this.claimsService = claimsService;
        }
        public async Task<CartViewModel?> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var currentUser = claimsService.GetCurrentUser;
            var cart = unitOfWork.Mapper.Map<CartEntity>(request.model);
            cart.UserId = currentUser;
            return await cartRepository.CreateCartAsync(cart);

        }
    }
}