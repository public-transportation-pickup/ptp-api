namespace PTP.Application.ViewModels.Stations;
public class StationRevenueModel : StationViewModel
{
    public decimal Revenue { get; set; } 
    public int OrderCompleted { get; set; }
    public int OrderCanceled { get; set; }
    public int OrderOthers { get; set; }
}