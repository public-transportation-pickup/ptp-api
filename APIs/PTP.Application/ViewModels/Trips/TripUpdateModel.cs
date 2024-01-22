using PTP.Domain.Enums;

namespace PTP.Application.ViewModels.Trips;
public class TripUpdateModel
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string StartTime { get; set; } = default!;
	public string Status { get; set; } = nameof(DefaultStatusEnum.Active);
}