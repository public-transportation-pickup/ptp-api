namespace PTP.Application.ViewModels.Products;

public class ProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
	public decimal Price { get; set; } = default!;
	public string Description { get; set; } = default!;
    public int PreparationTime { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
    public DateTime CreationDate { get; set; }
    public Guid StoreId { get; set; }
    public string StoreName{get;set;}=default!;
    public Guid CategoryId { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
}