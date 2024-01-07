using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PTP.Domain.Entities;
public class Route : BaseEntity
{
    public int RouteId { get; set; } = default!;
    public string RouteNo { get; set; } = string.Empty;
    public string Name { get; set; } = default!;
    public string Status { get; set; } = default!;
    public double Distance { get; set; } = 0d;

    public string RouteType { get; set; } = default!;
    public string TimeOfTrip { get; set; } = default!;
    public string HeadWay { get; set; } = default!;
    public string OperationTime { get; set; } = default!;
    public string OutBoundName { get; set; } = default!;
    public string NumOfSeats { get; set; } = default!;
    public string InBoundName { get; set; } = default!;
    public string OutBoundDescription { get; set; } = default!;
    public string InBoundDescription { get; set; } = string.Empty;
    public string TotalTrip { get; set; } = string.Empty;
    public string Orgs { get; set; } = default!;
    public string Tickets { get; set; } = default!;


    public ICollection<RouteStation> RouteStations { get; set; } = default!;

    public ICollection<RouteVar> RouteVars { get; set; } = default!;
    public ICollection<TimeTable> TimeTables { get; set; } = default!;


}