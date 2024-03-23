using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PTP.Domain.Entities.MongoDbs;
public class CartEntity
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string Note { get; set; } = string.Empty;
    // public Guid MenuId { get; set; } = Guid.Empty;
    public Guid StationId { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public bool IsCurrent { get; set; } = true;
    public List<CartItemEntity> Items { get; set; } = new();
}