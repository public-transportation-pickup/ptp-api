using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.MongoDbs.Carts;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.Features.Carts.Commands;
public class UpdateCartCommand : IRequest<CartViewModel?>
{
    public CartUpdateModel model { get; set; } = new();
    public class CommandHandler : IRequestHandler<UpdateCartCommand, CartViewModel?>
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
        public async Task<CartViewModel?> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = unitOfWork.Mapper.Map<CartEntity>(request.model);
            if (cart.Items.Any())
            {
                foreach (var item in cart.Items)
                {
                    if (await unitOfWork.ProductInMenuRepository.FirstOrDefaultAsync(x => x.Id == item.ProductMenuId) is null)
                    {
                        throw new Exception($"Product In Menu not exist in this Id {item.ProductMenuId}");
                    }
                }
            }
            var res = await cartRepository.UpdateCartAsync(cart);
            return res;
        }
    }
}