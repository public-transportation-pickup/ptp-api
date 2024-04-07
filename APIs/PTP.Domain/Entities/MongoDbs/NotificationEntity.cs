using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PTP.Domain.Entities.MongoDbs;
public enum NotificationSourceEnum
{
    Admin,
    Store
}
public class NotificationEntity
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string Title { get; set; } = string.Empty;
    public bool IsSeen { get; set; } = false;
    public string Content { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public NotificationSourceEnum Source { get; set; } = NotificationSourceEnum.Admin;
    public Guid UserId { get; set; } = Guid.Empty;
}

