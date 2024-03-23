using MediatR;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;

namespace PTP.Application.Features.Carts.Commands;
public class DeleteCartCommand : IRequest<bool>
{
    public class CommandHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        private readonly ICartRepository cartRepository;
        public CommandHandler(IUnitOfWork unitOfWork,
            IClaimsService claimsService,
            ICartRepository cartRepository)
        {
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;
            this.cartRepository = cartRepository;
        }

        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var currentUser = claimsService.GetCurrentUser;
            return currentUser != Guid.Empty
                && await cartRepository.DeleteCartASync(currentUser);

        }
    }
}