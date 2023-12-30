namespace PTP.Domain.Entities;

public class Menu : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;

	#region Relationship Configuration
	public Guid StoreId { get; set; }
	public Store Store { get; set; } = default!;
	public ICollection<ProductInMenu> ProductInMenus { get; set; } = default!;
	public ICollection<MenuCategory> MenuCategories { get; set; } = default!;
	#endregion
}

