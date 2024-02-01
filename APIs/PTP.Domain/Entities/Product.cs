using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class Product : BaseEntity
{
	public string Name { get; set; } = default!;
	[Precision(18, 2)]
	public decimal Price { get; set; } = default!;
	public string Description { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
	public int PreparationTime { get; set; } = default!;

    #region Config Relationship
    public Guid CategoryId { get; set; } = default!;
	public Category Category { get; set; } = default!;
	public Guid StoreId { get; set; }
	public Store Store { get; set; } = default!;
	public ICollection<ProductInMenu> ProductInMenus { get; set; } = default!;
    #endregion

}