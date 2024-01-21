using PTP.Application.ViewModels.Products;

namespace PTP.Application.ViewModels.Categories;

public class CategoryViewModel
{
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Status {  get; set; }=default!;
        public IEnumerable<ProductViewModel>? Products{get;set;}
}