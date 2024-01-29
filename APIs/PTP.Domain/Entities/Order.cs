using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Order : BaseEntity
{
	public string Name { get; set; } = default!;
	public string PhoneNumber {  get; set; }=default!;
	[Precision(18, 2)]	
	public decimal Total { get; set; } = 0;
	public string PickUpTime {  get; set; } = default!;
	public string? CanceledReason {  get; set; }
	public string Status { get; set; } = nameof(OrderStatusEnum.Payed)!;
	[Precision(18, 2)]

	#region Relationship Configuration
	public Guid UserId { get; set; }
	public User User { get; set; } = default!;
	public Guid StationId { get; set; } = default!;
	public Station Station { get; set; } = default!;

	public Guid StoreId { get; set; }
	public Store Store { get; set; } = default!;

	public ICollection<OrderDetail> OrderDetails { get; set; } = default!;
	public Guid PaymentId { get; set; } = default!;
	public Payment Payment { get; set; } = default!;
	#endregion
}