using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class RouteVar : BaseEntity
{

	/* 
    int RouteId, int RouteVarId, string RouteVarName, 
string RouteVarShortName, string RouteNo, string StartStop, 
string EndStop, double Distance, bool OutBound, int RunningTime
    */
	public int RouteVarId { get; set; } = default!;
	public string RouteVarName { get; set; } = default!;
	public string RouteVarShortName { get; set; } = default!;
	public string StartStop { get; set; } = default!;
	public string EndStop { get; set; } = default!;
	public bool OutBound { get; set; } = default!;
	public int RunningTime { get; set; } = default!;
	public double Distance { get; set; } = default!;
	[Precision(18, 2)]
	public double AverageVelocity { get; set; } = default!;
	#region  RelationshipConfiguration
	public Guid RouteId { get; set; } = default!;
	public Route Route { get; set; } = default!;
	//public ICollection<Path> Paths { get; set; } = default!;
	public ICollection<TimeTable> Timetables { get; set; } = default!;
	public ICollection<RouteStation> RouteStations { get; set; } = default!;
	#endregion

}