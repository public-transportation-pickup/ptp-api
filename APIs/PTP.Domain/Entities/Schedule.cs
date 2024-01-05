using System.Net;

namespace PTP.Domain.Entities;
public class Schedule : BaseEntity
{
    public DateTime ArrivalTime { get; set; } = default!;
    public string Status { get; set; } = default!;

    #region RelationshipConfiguration
    public Guid TripId { get; set; } = default!;
    public Trip Trip { get; set; } = default!;
    public Guid StationId { get; set; } = default!;
    public Station Station { get; set; } = default!;
    #endregion
}