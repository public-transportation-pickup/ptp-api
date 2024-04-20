namespace PTP.Application.ViewModels.Routes;
public record RouteViewModel
(
    Guid Id,
    string RouteNo,
    string Name,
    string Status,
    double Distance,
    string TimeOfTrip,
    string HeadWay,
    string OperationTime,
    string NumOfSeats,
    string InboundName,
    string OutBoundName,
    string InBoundDescription,
    string OutBoundDescription,
    string TotalTrip,
    string Orgs,
    string Tickets
);