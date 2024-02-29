using Microsoft.AspNetCore.Http;
using PTP.Application.ViewModels.Products;

namespace PTP.Application.ViewModels.Categories;

public class CategoryCreateModel
{
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Status { get; set; } = default!;
        public IFormFile? Image { get; set; }
}