using PTP.Application.ViewModels.Stores;
using PTP.Domain.Enums;

namespace PTP.Application.ViewModels.Menus;

public class MenuViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TimeSpan StartTime { get; set; } = default!;
    public TimeSpan EndTime { get; set; } = default!;
    public string DateFilter { get; set; } = default!;
    public string Status { get; set; } =default!;
    public Guid StoreId { get; set; }
    public DateTime CreationDate { get; set; }
    public StoreViewModel Store { get; set; } = default!;
    //public ICollection<ProductInMenu> ProductInMenus { get; set; }

}