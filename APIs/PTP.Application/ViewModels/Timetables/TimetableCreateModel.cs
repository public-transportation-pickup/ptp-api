namespace PTP.Application.ViewModels.Timetables;
public class TimetableCreateModel
{
    public string ApplyDates { get; set; } = default!;
    public Guid RouteId { get; set; } = default!;
    public Guid RouteVarId { get; set; } = default!;
}