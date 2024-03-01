using MediatR;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.Services.RouteStations;
using PTP.Application.ViewModels.RouteStations;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Queries;
public class GetTripByUserLocation : IRequest<TripViewModel>
{
    public Guid RouteVarId { get; set; } = Guid.Empty;
    public decimal Latitude { get; set; } = 0;
    public decimal Longitude { get; set; } = 0;
    public class QueryHandler : IRequestHandler<GetTripByUserLocation, TripViewModel?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RouteStationBusinesses routeStationBusinesses;
        private readonly ILocationService locationService;
        public QueryHandler(IUnitOfWork unitOfWork, ILocationService locationService)
        {
            this.locationService = locationService;
            this.routeStationBusinesses = new(unitOfWork);
            this.unitOfWork = unitOfWork;
        }
        public async Task<TripViewModel?> Handle(GetTripByUserLocation request, CancellationToken cancellationToken)
        {
            var timeTable = (await unitOfWork.TimeTableRepository.WhereAsync(x => x.RouteVarId == request.RouteVarId && x.IsCurrent)
                ?? throw new Exception("Not found timetable")).First();
            var getRouteVarTask = unitOfWork.RouteVarRepository.GetByIdAsync(request.RouteVarId, x => x.Route);
            var getRouteStationTask = routeStationBusinesses.GetRouteStationByRouteVarId(request.RouteVarId);
            var tripTask = unitOfWork.TripRepository.WhereAsync(x => x.TimeTableId == timeTable.Id);
            var currentCoordinate = new { request.Latitude, request.Longitude };
            await Task.WhenAll(getRouteStationTask, tripTask, getRouteVarTask);

            var routeStations = getRouteStationTask.Result;
            var routeVar = getRouteVarTask.Result;
            var trips = tripTask.Result;
            var now = DateTime.Now;
            if(routeVar?.Route?.AverageVelocity <= 0)
            { 
                throw new Exception($"RouteVaration: {routeVar.RouteVarName} chưa support tính duration! add endpoint 'distance-modification'");
            }
            if (routeStations.Count > 0 && trips.Count > 0 && routeVar?.Route?.AverageVelocity > 0) 
            {
                // ToDo
                var points = RouteStationViewModel.To(routeStations);
                var upperIndex = FindUpperBoundIndex(points, currentCoordinate.Latitude, currentCoordinate.Longitude);
                var nextStation = routeStations.FirstOrDefault(x => x.Index == upperIndex) ??
                    throw new NullReferenceException();
                var distance = await locationService.GetDistance(orgLat: currentCoordinate.Latitude, 
                    orgLng: currentCoordinate.Longitude,
                    destLat: nextStation.Latitude,
                    destLng: nextStation.Longitude);
                var duration = distance / routeVar.Route.AverageVelocity;
                
                foreach(var rs in routeStations.Where(x => x.Index >= nextStation.Index))
                {
                    duration += rs.DurationToNext;
                }
                var eta = DateTime.Now.AddMinutes(duration);

                var currentEstimateTrip = trips.OrderBy(x => Math.Abs((eta - DateTime.Parse(x.EndTime)).TotalMilliseconds)).First();
                return unitOfWork.Mapper.Map<TripViewModel>(currentEstimateTrip);

            } else return null;


        }
        
        private static int FindLowerBoundIndex(decimal[,] points,
            decimal lat,
            decimal lon)
        {
            int left = 0;
            int right = points.GetLength(0) - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (points[mid, 0] < lat || (points[mid, 0] == lat && points[mid, 1] < lon))
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return right;
        }
        private static int FindUpperBoundIndex(decimal[,] points,
            decimal lat,
            decimal lon)
        {
            int left = 0;
            int right = points.GetLength(0) - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (points[mid, 0] > lat || (points[mid, 0] == lat && points[mid, 1] > lon))
                    right = mid - 1;
                else
                    left = mid + 1;
            }

            return left;
        }

    }
}