using System.Diagnostics;
using System.Dynamic;

namespace PTP.Domain.Entities;
public class RouteStation : BaseEntity
{
    public byte Index { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public double DistanceFromStart { get; set; } = default!;
    public double DistanceToNext { get; set; } = default!;
    public double DurationToNext { get; set; } = default!;
    public double DurationFromStart { get; set; } = default!;

    #region Relationship Configuration
    public Guid RouteId { get; set; }
    public Route Route { get; set; } = default!;
    public Guid StationId { get; set; }
    public Station Station { get; set; } = default!;
    #endregion
}