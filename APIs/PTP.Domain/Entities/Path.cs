// using Microsoft.EntityFrameworkCore;

// namespace PTP.Domain.Entities;
// public class Path : BaseEntity
// {
//     [Precision(18, 2)]
//     public List<decimal> Lat { get; set; } = default!;
//     [Precision(18, 2)]
//     public List<decimal> Lng { get; set; } = default!;

//     #region Config Relationship
//     public Guid RouteId { get; set; } = default!;
//     public Route Route { get; set; } = default!;
//     public Guid RouteVarId {get; set;} = default!;
//     public RouteVar RouteVar {get ;set;} = default!;
//     #endregion

// }