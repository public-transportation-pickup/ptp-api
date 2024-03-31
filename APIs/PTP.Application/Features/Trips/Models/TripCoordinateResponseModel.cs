using PTP.Application.ViewModels.Schedules;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Models;
public class TripCoordinateResponseModel
{
    public TripViewModel Trip { get; set; } = new();
    public ScheduleViewModel Schedule { get; set; } = new();

}