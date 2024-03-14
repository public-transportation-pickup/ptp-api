
using Microsoft.AspNetCore.Http;

namespace PTP.Application.ViewModels.Products;

public class ProductCreateModel
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public string Description { get; set; } = default!;
    public IFormFile? Image { get; set; }
    public int PreparationTime { get; set; } = default!;
    public int NumProcessParallel { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid CategoryId { get; set; } = default!;
    public Guid StoreId { get; set; }
}