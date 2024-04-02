namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartUpdateModel
{
    public string Id { get; set; } = string.Empty;
    public Guid StoreId { get; set; }
    public Guid StationId { get; set; } = Guid.Empty;
    public string StationAddr { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime PickUpTime { get; set; }
    public decimal Total { get; set; }
    // public Guid MenuId { get; set; } = Guid.Empty;
    public List<CartItemUpdateModel> Items { get; set; } = new();
}