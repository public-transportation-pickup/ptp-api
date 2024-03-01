using Microsoft.Extensions.Logging;

namespace PTP.Application.Services.RouteStations;
public partial class RouteStationBusinesses
{
    private readonly IUnitOfWork unitOfWork;

    public RouteStationBusinesses(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
}