
namespace PTP.Application.ViewModels.MongoDbs.Carts;
public class CartItemCreateModel
{
    
    public Guid ProductInMenuId { get; set; } = Guid.Empty;
    public int Quantity { get; set; } = 0;
    public string Note { get; set; } = string.Empty;
}