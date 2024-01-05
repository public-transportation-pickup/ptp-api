using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class Station : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int RouteNumber { get; set; } = 0;
    public string Status { get; set; } = default!;
    #region Location 
    public string Address { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Latitude { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Longitude { get; set; } = default!;
    #endregion
    #region Relationship
    public ICollection<StoreStation> StoreStations { get; set; } = default!;
    public ICollection<RouteStation> RouteStations { get; set; } = default!;
    public ICollection<Schedule> Schedules { get; set; } = default!;
    #endregion
}