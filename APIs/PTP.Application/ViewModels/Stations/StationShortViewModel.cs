namespace PTP.Application.ViewModels.Stations;
public class StationShortViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public int Index { get; set; } = 0;
    public string Address { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public decimal Latitude { get; set; } = default!;

    public decimal Longitude { get; set; } = default!;
}