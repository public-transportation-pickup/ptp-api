using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PTP.Domain.Entities;
public class Route : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Distance { get; set; } = 0d;
    public int RouteNumber { get; set; } = 0;

    public int RouteType { get; set; } = default!;
    public int TimeOfTrip { get; set; } = default!;
    public int HeadWay { get; set; } = default!;
    public string OperationTime { get; set; } = default!;
    public string OutBoundName { get; set; } = default!;
    public string InBoundName { get; set; } = default!;
    public string OutBoundDescription { get; set; } = default!;
    public string InBoundDescription { get; set; } = string.Empty;
    public int TotalTrip { get; set; } = 1;


    public ICollection<RouteStation> RouteStations { get; set; } = default!;
    public ICollection<Trip> Trips { get; set; } = default!;

}