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

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string DateApply { get; set; } = default!;
    public string Status { get; set; } = default!;
    public Guid StoreId { get; set; }
    public DateTime CreationDate { get; set; }
    public StoreViewModel Store { get; set; } = default!;
    public bool IsApplyForAll { get; set; } = false;

    public List<object>? Categories { get; set; }
    public IEnumerable<ProductMenuViewModel>? ProductInMenus { get; set; }

}
public class ProductsStore
{
    public Guid StoreId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Status { get; set; } = default!;

    public List<object>? Categories { get; set; }
    public IEnumerable<ProductMenuViewModel>? ProductInMenus { get; set; }

}