using MediatR;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.MongoDbs.Carts;

namespace PTP.Application.Features.Carts.Queries;
public class GetCartByUserQuery : IRequest<CartViewModel?>
{
    public class QueryHandler : IRequestHandler<GetCartByUserQuery, CartViewModel?>
    {
        private readonly IClaimsService claimsService;
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;
        public QueryHandler(IClaimsService claimsService,
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;
            this.cartRepository = cartRepository;
        }
        public async Task<CartViewModel?> Handle(GetCartByUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = claimsService.GetCurrentUser;
            var result = await cartRepository.GetCartByUserIdAsync(currentUser);
            return result ?? new();
        }
    }
}