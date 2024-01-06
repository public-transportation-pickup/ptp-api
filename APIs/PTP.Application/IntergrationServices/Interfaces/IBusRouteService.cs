namespace PTP.Application.IntergrationServices.Interfaces;
public interface IBusRouteService 
{
    Task<bool> TryCreateRouteAsync(int routeId);
    Task CheckNewCreatedRoute();

}