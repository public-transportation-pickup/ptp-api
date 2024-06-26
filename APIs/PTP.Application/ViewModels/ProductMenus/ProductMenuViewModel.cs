namespace PTP.Application.ViewModels.ProductMenus;

public class ProductMenuViewModel
{
    public Guid Id { get; set; }
    public string Status { get; set; } = default!;
    public decimal SalePrice { get; set; } = 0;
    public int QuantityInDay { get; set; }
    public int QuantityUsed { get; set; }
    public int PreparationTime { get; set; } = default!;
    public int NumProcessParallel { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid MenuId { get; set; }
    public string MenuName { get; set; } = default!;
    public string MenuDescription { get; set; } = default!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal ProductPrice { get; set; } = default!;
    public string ProductDescription { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
    public Guid CategoryId { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public Guid StoreId { get; set; }
}

public class ProductMenuDelete
{
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }

}