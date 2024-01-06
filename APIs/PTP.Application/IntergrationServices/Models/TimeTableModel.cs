using Microsoft.Identity.Client;

namespace PTP.Application.IntergrationServices.Models;
public record TimeTableModel(int RouteId, int RouteVarId, int TimeTableId, 
string RouteVarShortName, string StartDate, string EndDate, 
string StartStop, string EndStop, bool IsCurrent, 
string RunningTime, string HeadWay, int TotalTrip, 
string OperationTime, string ApplyDates);