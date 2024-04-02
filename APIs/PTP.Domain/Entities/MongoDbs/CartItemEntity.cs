using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PTP.Domain.Entities.MongoDbs;
public class CartItemEntity
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string Name { get; set; } = string.Empty;
    public decimal ActualPrice { get; set; }
    public int Quantity { get; set; }
    public int MaxQuantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public Guid ProductMenuId { get; set; }
}