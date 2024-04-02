namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartItemUpdateModel
{
    public Guid ProductMenuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int MaxQuantity { get; set; }
    public decimal ActualPrice { get; set; }
    public string ImageURL { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}