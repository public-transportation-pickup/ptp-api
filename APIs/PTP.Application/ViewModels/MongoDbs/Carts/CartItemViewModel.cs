namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public Guid ProductInMenuId { get; set; }
    public int Quantity { get; set; }
    public decimal ActualPrice { get; set; }
    public string Note { get; set; } = string.Empty;
}