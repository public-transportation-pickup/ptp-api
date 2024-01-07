using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class ProductInMenu : BaseEntity
{
    public string Status { get; set; } = nameof(ProductInMenu);
    [Precision(18, 2)]
    public decimal ActualPrice { get; set; } = 0;

    #region Relationship Configuration 
    public Guid MenuId { get; set; }
    public Menu Menu { get; set; } = default!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = default!;
    #endregion
}