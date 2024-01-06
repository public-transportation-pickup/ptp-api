namespace PTP.Application.IntergrationServices.Models;
public record RouteVarModel(int RouteId, int RouteVarId, string RouteVarName, 
string RouteVarShortName, string RouteNo, string StartStop, 
string EndStop, double Distance, bool OutBound, int RunningTime);