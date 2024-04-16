using PTP.Application.ViewModels.Orders;

namespace PTP.Application.ViewModels.Products;
public class TopProductModel
{
    public Guid ProductMenuId { get; set; } = Guid.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal ActualPrice { get; set; }
    public int OrderCount { get; set; }
}

public class TopProductViewModel
{
    public List<TopProductModel> Products { get; set; } = new();
    public List<OrderViewModel> Orders { get; set; } = new();
}