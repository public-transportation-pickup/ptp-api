namespace PTP.Application.ViewModels.Trips;
public class TripCreateModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public Guid TimeTableId { get; set; } = Guid.Empty;
}