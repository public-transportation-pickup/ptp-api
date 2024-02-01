namespace PTP.Application.ViewModels.Stations;
public class StationShortViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public int Index { get; set; } = 0;
}