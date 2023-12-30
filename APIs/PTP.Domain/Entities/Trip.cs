using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class Trip : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public byte StartTime { get; set; } = default!;
    public byte EndTime { get; set; } = default!;
    public byte TimeGap { get; set; } = default!; // Time between two trips, by minutes?
    [Precision(18, 2)]
    public decimal Price { get; set; } = default!;
    public int NumberOfTrip { get; set; } = default!;
    public string Status { get; set; } = default!;


    #region Relationship Configuration
    public Guid RouteId { get; set; } = default!;
    public Route Route { get; set; } = default!;
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = default!;
    #endregion


}