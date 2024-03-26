using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PTP.Domain.Entities.MongoDbs;
public class CartItemEntity
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public decimal ActualPrice { get; set; }
    public int Quantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public Guid ProductMenuId { get; set; }
}