using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class Station : BaseEntity
{
    public int StopId { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string StopType { get; set; } = default!;
    public string Name { get; set; } = default!;

    public string Status { get; set; } = default!;
    public string Zone { get; set; } = default!;
    public string Ward { get; set; } = default!;
    public string AddressNo { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string SupportDisability { get; set; } = default!;

    #region Location 
    public string Address { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Latitude { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Longitude { get; set; } = default!;
    #endregion
    #region Relationship
    public Guid? StoreId { get; set; } = default!;
    public Store? Store { get; set; } = default!;
    public ICollection<Order> Orders { get; set; } = default!;
    public ICollection<RouteStation> RouteStations { get; set; } = default!;
    public ICollection<Schedule> Schedules { get; set; } = default!;
    #endregion
}