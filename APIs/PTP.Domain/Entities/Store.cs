using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Store : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string PhoneNumber { get; set; } = default!;
	public string Status { get; set; } = nameof(StoreStatusEnum.ACTIVE)!;
    public TimeSpan OpenedTime { get; set; } = default!;
    public TimeSpan ClosedTime { get; set; } = default!;
    [Precision(18, 10)]
	public decimal Latitude { get; set; } = default!;
	[Precision(18, 10)]
	public decimal Longitude { get; set; } = default!;
    public string Zone { get; set; } = default!;
    public string Ward { get; set; } = default!;
    public string AddressNo { get; set; } = default!;
    public string Street { get; set; } = default!;
    public DateTime? ActivationDate { get; set; } = null;

	#region Image 
	public string ImageName { get; set; } = string.Empty;
	public string ImageURL { get; set; } = string.Empty;
	#endregion

	#region  Relationship Configuration
	public ICollection<Station> Stations { get; set; } = default!;
	public ICollection<Product> Products { get; set; } = default!;
	public ICollection<Menu> Menus { get; set; } = default!;
	//public ICollection<Wallet> Wallets { get; set; } = default!;
	public Guid? WalletId { get; set; }
	public Wallet? Wallet { get; set; } = default!;
	public Guid UserId { get; set; }
	public User User { get; set; } = default!;

	public ICollection<Order> Orders { get; set; } = default!;
	#endregion
}