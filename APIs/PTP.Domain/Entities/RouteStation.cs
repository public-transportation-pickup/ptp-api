using System.Diagnostics;
using System.Dynamic;

namespace PTP.Domain.Entities;
public class RouteStation : BaseEntity
{
    public string Name { get; set; } = default!;
    public byte Index { get; set; } = 0;

    #region Relationship Configuration
    public Guid RouteId { get; set; }
    public Route Route { get; set; } = default!;
    public Guid StationId { get; set; }
    public Station Station {get; set;} = default!;
    #endregion
}