namespace PTP.Domain.Entities;
public class ProductInMenu : BaseEntity
{
    public string Status { get; set; } = default!;
    public int UnitInStock { get; set; } = default!;

    #region Relationship Configuration 
    public Guid MenuId { get; set; }
    public Menu Menu { get; set; } = default!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    #endregion
}