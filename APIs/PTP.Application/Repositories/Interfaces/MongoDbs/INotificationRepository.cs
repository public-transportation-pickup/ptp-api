using MongoDB.Bson;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.Repositories.Interfaces.MongoDbs;
public interface INotificationRepository
{
    public Task<List<NotificationEntity>?> GetAll(CancellationToken cancellationToken = default);
    public Task<List<NotificationEntity>?> CreateManyAsync(List<NotificationEntity>? entities, 
        CancellationToken cancellationToken = default);
    public Task<List<NotificationEntity>?> GetByUser(Guid userId,
        CancellationToken cancellationToken = default);
    public Task<bool> DeleteAllAsync(Guid userId, 
        CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
    public Task<NotificationEntity?> CreateAsync(NotificationEntity entity,
        CancellationToken cancellationToken = default);
    Task<NotificationEntity?> UpdateAsync(NotificationEntity entity,
        CancellationToken cancellationToken = default);

}