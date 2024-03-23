using PTP.Application.ViewModels.MongoDbs.Carts;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.Repositories.Interfaces.MongoDbs;
public interface ICartRepository
{
    public Task<CartViewModel?> CreateCartAsync(CartEntity model);
    public Task<CartViewModel?> GetCartByUserIdAsync(Guid userId);
    public Task<bool> DeleteCartASync(Guid userId);
    public Task<CartViewModel?>UpdateCartAsync(CartEntity entity);

}