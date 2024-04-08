using System;
using MongoDB.Bson;
using MongoDB.Driver;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Infrastructure.Repositories.MongoDbs;
public class NotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<NotificationEntity> colletions;

    public NotificationRepository(IMongoDatabase db)
    {
        this.colletions = db.GetCollection<NotificationEntity>("notifications");
    }

    public async Task<NotificationEntity?> CreateAsync(NotificationEntity entity,
        CancellationToken cancellationToken = default)
    {
        await this.colletions.InsertOneAsync(entity,
            cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<List<NotificationEntity>?> CreateManyAsync(List<NotificationEntity>? entities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        await colletions.InsertManyAsync(documents: entities,
            cancellationToken: cancellationToken);
        return entities;
    }

    public async Task<bool> DeleteAllAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var result = await colletions.DeleteManyAsync(
            filter: Builders<NotificationEntity>.Filter.Eq(x => x.UserId, userId),
            cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var result = await colletions.DeleteOneAsync(
            filter: Builders<NotificationEntity>.Filter.Eq(x => x.Id, id),
            cancellationToken: cancellationToken
        );
        return result.DeletedCount > 0;
    }

    public async Task<List<NotificationEntity>?> GetAll(CancellationToken cancellationToken = default)
    {
        return await colletions.Find(
            Builders<NotificationEntity>.Filter.Empty
        ).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<NotificationEntity>?> GetByUser(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = (await colletions.FindAsync(x => x.UserId == userId)).ToList();
        return result;
    }

    public async Task<NotificationEntity?> UpdateAsync(NotificationEntity entity,
        CancellationToken cancellationToken = default)
    {
        var result = await colletions.FindOneAndUpdateAsync(
            filter: Builders<NotificationEntity>.Filter.Eq(x => x.Id, entity.Id),
            update: Builders<NotificationEntity>.Update
                .Set(x => x.ImageURL, entity.ImageURL)
                .Set(x => x.Content, entity.Content)
                .Set(x => x.Title, entity.Title)
                .Set(x => x.IsSeen, entity.IsSeen)
                .Set(x => x.Source, entity.Source));
        return result;
    }
}