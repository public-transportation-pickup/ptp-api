namespace PTP.Application.IntergrationServices.Interfaces;
public interface ILocationService
{
	Task<double> GetDistance(decimal orgLat, decimal orgLng, decimal destLat, decimal destLng, string travelMode = "car");
}