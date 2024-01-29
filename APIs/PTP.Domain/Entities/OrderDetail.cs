using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class OrderDetail : BaseEntity
{
	[Precision(18, 2)]
	public decimal ActualPrice { get; set; } = 0;
	public int Quantity { get; set; } = 1;
	public string Note { get; set; } = default!;
	#region RelationshipConfiguration
	public Guid ProductMenuId { get; set; }
	public ProductInMenu ProductInMenu { get; set; } = default!;
	public Guid OrderId { get; set; } = default!;
	public Order Order { get; set; } = default!;
	#endregion
}