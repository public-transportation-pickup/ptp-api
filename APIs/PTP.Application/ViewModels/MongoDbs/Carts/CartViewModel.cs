namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    // public Guid MenuId { get; set; } = Guid.Empty;
    public Guid StationId { get; set; } = Guid.Empty;
    public Guid StoreId {get ;set;} 
    public Guid UserId { get; set; } = Guid.Empty;
    public bool IsCurrent { get; set; } = true;
    public List<CartItemViewModel> Items { get; set; } = new();
}