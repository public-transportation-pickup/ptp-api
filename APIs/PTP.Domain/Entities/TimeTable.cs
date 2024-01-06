namespace PTP.Domain.Entities;
public class TimeTable : BaseEntity
{
    public int TimeTableId { get; set; } = default!;
    // public DateTime StartDate { get; set; } = default!;
    // public DateTime EndDate { get; set; } = default!;
    public bool IsCurrent { get; set; } = default!;
    public string ApplyDates { get; set; } = default!;
    /* 
    int RouteId, int RouteVarId, int TimeTableId, 
string RouteVarShortName, string StartDate, string EndDate, 
string StartStop, string EndStop, bool IsCurrent, 
string RunningTime, string HeadWay, int TotalTrip, 
string OperationTime, string ApplyDates
    */
    #region RelationshipConfiguration
    public Guid RouteVarId { get; set; } = default!;
    public RouteVar RouteVar { get; set; } = default!;
    public ICollection<Trip> Trips { get; set; } = default!;
    #endregion
}