using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class ProductInMenu : BaseEntity
{
	public string Status { get; set; } = nameof(ProductInMenu);
	public int NumProcessParallel { get; set; }
	[Precision(18, 2)]
	public decimal SalePrice { get; set; } = 0;
	public int QuantityInDay { get; set; }
	public int QuantityUsed { get; set; } = 0;
	public int PreparationTime { get; set; }

	#region Relationship Configuration 
	public Guid MenuId { get; set; }
	public Menu Menu { get; set; } = default!;
	public Guid ProductId { get; set; }
	public Product Product { get; set; } = default!;

	public ICollection<OrderDetail> OrderDetails { get; set; } = default!;
	#endregion
}