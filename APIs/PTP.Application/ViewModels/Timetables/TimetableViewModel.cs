namespace PTP.Application.ViewModels.Timetables;
public class TimetableViewModel
{
    public Guid Id { get; set; }
    public int TimeTableId { get; set; } = default!;
    public bool IsCurrent { get; set; } = default!;
    public string ApplyDates { get; set; } = default!;
    public Guid RouteId { get; set; }
    public Guid RouteVarId { get; set; }
}