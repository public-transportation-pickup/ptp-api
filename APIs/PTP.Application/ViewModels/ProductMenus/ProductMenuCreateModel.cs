using PTP.Domain.Entities;

namespace PTP.Application.ViewModels.ProductMenus;

public class ProductMenuCreateModel
{
    public Guid ProductId { get; set; }
    public Guid MenuId { get; set; }
    public string Status { get; set; } =default!;
	public decimal ActualPrice { get; set; } = 0;
}
