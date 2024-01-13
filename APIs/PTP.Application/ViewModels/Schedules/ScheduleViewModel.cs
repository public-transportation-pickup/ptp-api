using System.Diagnostics;

namespace PTP.Application.ViewModels.Schedules;
public class ScheduleViewModel
{
    public int Index { get; set; }
    public double DistanceFromStart { get; set; } = default!;
    public double DistanceToNext { get; set; } = default!;
    public decimal DurationFromStart { get; set; } = default!;
    public decimal DurationToNext { get; set; } = default!;
    public string StationName { get; set; } = default!;
    public TimeSpan ArrivalTime { get; set; } = default!;

}