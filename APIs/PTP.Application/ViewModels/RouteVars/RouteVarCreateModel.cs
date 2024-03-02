using Microsoft.AspNetCore.Http.Connections;

namespace PTP.Application.ViewModels.RouteVars;
public class RouteVarCreateModel
{
	public int RouteVarId { get; set; }
	public string RouteVarName { get; set; } = string.Empty;
	public string RouteVarShortName { get; set; } = default!;
	public string StartStop { get; set; } = default!;
	public string EndStop { get; set; } = default!;
	public bool OutBound { get; set; } = true;
	public int RunningTime { get; set; } = 0;
	public List<RouteStationModel> RouteStations { get; set; } = new();
	public Guid RouteId { get; set; } = default!;
}

public class RouteStationModel
{
	public Guid StationId { get; set; }
	public string? Description { get; set; } = string.Empty;
	public int Index { get; set; } = 0;
}