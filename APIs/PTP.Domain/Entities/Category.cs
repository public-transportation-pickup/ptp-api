using System.ComponentModel.DataAnnotations;

namespace PTP.Domain.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
    public string Status { get; set; } = default!;
    public ICollection<Product> Products { get; set; } = default!;


}