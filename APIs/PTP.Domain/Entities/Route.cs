using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PTP.Domain.Entities;
public class Route : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Distance { get; set; } = 0d;
    public string RouteType { get; set; } = default!;

    public ICollection<RouteStation> RouteStations {get; set;} = default!;
    public ICollection<Trip> Trips {get; set;} = default!;

}