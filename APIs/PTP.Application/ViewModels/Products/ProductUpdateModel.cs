namespace PTP.Application.ViewModels.Products;
public class ProductUpdateModel : ProductCreateModel
{
   public Guid Id { get; set; }
   public Guid ProductMenuId { get; set; }
   public string Status { get; set; } = default!;

}
