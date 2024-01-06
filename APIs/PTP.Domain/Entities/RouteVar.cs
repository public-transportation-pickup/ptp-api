namespace PTP.Domain.Entities;
public class RouteVar : BaseEntity
{

    /* 
    int RouteId, int RouteVarId, string RouteVarName, 
string RouteVarShortName, string RouteNo, string StartStop, 
string EndStop, double Distance, bool OutBound, int RunningTime
    */
    public int RouteVarId { get; set; } = default!;
    public string RouteVarName { get; set; } = default!;
    public string RouteVarShortName { get; set; } = default!;
    public string StartStop { get; set; } = default!;
    public string EndStop { get; set; } = default!;
    public bool OutBound { get; set; } = default!;
    public int RunningTime { get; set; } = default!;
    #region  RelationshipConfiguration
    public Guid RouteId { get; set; } = default!;
    public Route Route { get; set; } = default!;
    public Guid TimeTableId { get; set; } = default!;
    public TimeTable TimeTable { get; set; } = default!;
    #endregion

}