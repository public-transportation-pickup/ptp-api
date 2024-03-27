using MongoDB.Driver;
using PTP.Application;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.ViewModels.MongoDbs.Carts;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Infrastructure.Repositories.MongoDbs;
public class CartRepository : ICartRepository
{
    private readonly IMongoCollection<CartEntity> cartCollection;
    private readonly IUnitOfWork unitOfWork;
    public CartRepository(AppSettings appSettings, IUnitOfWork unitOfWork)
    {
        MongoClient client = new MongoClient(appSettings.ConnectionStrings.MongoDbConnection);
        IMongoDatabase db = client.GetDatabase("ptp-db");
        cartCollection = db.GetCollection<CartEntity>("carts");
        this.unitOfWork = unitOfWork;
    }


    public async Task<bool> DeleteCartASync(Guid userId)
    {
        var cart = await cartCollection.FindOneAndUpdateAsync(x => x.UserId == userId && x.IsCurrent,
            Builders<CartEntity>.Update.Set(x => x.IsCurrent, false));
        return cart is not null;
    }

    public async Task<CartViewModel?> GetCartByUserIdAsync(Guid userId)
    {
        var cart = (await cartCollection.FindAsync(x => x.UserId == userId && x.IsCurrent)).FirstOrDefault();
        return unitOfWork.Mapper.Map<CartViewModel>(cart);
    }

    public async Task<CartViewModel?> UpdateCartAsync(CartEntity entity)
    {
        var result = await cartCollection.UpdateOneAsync(
            Builders<CartEntity>.Filter.
                Eq(x => x.Id, entity.Id),
            Builders<CartEntity>.Update
                .Set(x => x.Items, entity.Items)
                .Set(x => x.Note, entity.Note),
                options: new UpdateOptions { IsUpsert = true }
        );
        return await GetCartByUserIdAsync(entity.UserId);

    }

    async Task<CartViewModel?> ICartRepository.CreateCartAsync(CartEntity model)
    {
        await cartCollection.InsertOneAsync(model);
        return await GetCartByUserIdAsync(model.UserId) ?? new();
    }
}