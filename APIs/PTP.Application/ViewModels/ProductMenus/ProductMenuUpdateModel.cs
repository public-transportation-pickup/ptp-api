using PTP.Domain.Entities;

namespace PTP.Application.ViewModels.ProductMenus;

public class ProductMenuUpdateModel
{
    public Guid Id { get; set; }
    public string Status { get; set; } = default!;
    public decimal SalePrice { get; set; } = 0;
    public int QuantityInDay { get; set; }
    public int NumProcessParallel { get; set; }
    public int PreparationTime { get; set; }

}
