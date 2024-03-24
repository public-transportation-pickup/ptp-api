
namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartItemCreateModel
{

    public Guid ProductMenuId { get; set; } = Guid.Empty;
    public int Quantity { get; set; } = 0;
    public decimal ActualPrice { get; set; }
    public string Note { get; set; } = string.Empty;
}