
namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartItemCreateModel
{

    public Guid ProductMenuId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public decimal ActualPrice { get; set; }
    public int MaxQuantity { get; set; }
    public string ImageURL { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}