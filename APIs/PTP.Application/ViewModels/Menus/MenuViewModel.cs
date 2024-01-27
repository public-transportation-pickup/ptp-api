using PTP.Application.ViewModels.ProductMenus;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Enums;

namespace PTP.Application.ViewModels.Menus;

public class MenuViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string StartTime { get; set; } = default!;
    public string EndTime { get; set; } = default!;
    public string DateFilter { get; set; } = default!;
    public string Status { get; set; } =default!;
    public Guid StoreId { get; set; }
    public DateTime CreationDate { get; set; }
    public StoreViewModel Store { get; set; } = default!;
    public IEnumerable<ProductMenuViewModel>? ProductInMenus { get; set; }

}