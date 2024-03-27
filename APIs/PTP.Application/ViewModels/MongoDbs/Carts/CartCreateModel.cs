
namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartCreateModel
{
    public Guid StationId { get; set; } = Guid.Empty;
    public Guid StoreId {get ;set;} = Guid.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime PickUpTime { get; set; }
    public decimal Total { get; set; }
    // public Guid MenuId { get; set; } = Guid.Empty;
    public List<CartItemCreateModel> Items { get; set; } = new();
}