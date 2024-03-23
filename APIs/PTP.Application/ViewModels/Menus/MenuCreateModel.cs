namespace PTP.Application.ViewModels.Menus;

public class MenuCreateModel
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string StartTime { get; set; } = default!;
    public string EndTime { get; set; } = default!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string DateApply { get; set; } = default!;
    public string Status { get; set; } = default!;
    public Guid StoreId { get; set; }
}