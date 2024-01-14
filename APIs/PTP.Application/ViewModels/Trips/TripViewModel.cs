using PTP.Application.ViewModels.Schedules;

namespace PTP.Application.ViewModels.Trips;
public class TripViewModel
{
    public Guid Id { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string StartTime { get; set; } = default!;
    public string EndTime { get; set; } = default!;
    public string ApplyDates { get; set; } = default!;
    public IEnumerable<ScheduleViewModel> Schedules { get; set; } = default!;
    public string Status { get; set; } = string.Empty;
}