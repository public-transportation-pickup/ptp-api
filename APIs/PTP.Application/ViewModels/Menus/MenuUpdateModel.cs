namespace PTP.Application.ViewModels.Menus;

public class MenuUpdateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string StartTime { get; set; } = default!;
    public string EndTime { get; set; } = default!;
    public DateTime DateApply { get; set; } = default!;
    public string Status { get; set; } = default!;
    public Guid StoreId { get; set; }
}