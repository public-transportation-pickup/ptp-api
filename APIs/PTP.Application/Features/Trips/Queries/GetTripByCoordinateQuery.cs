using MediatR;
using PTP.Application.Features.Trips.Models;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.Services.RouteStations;
using PTP.Application.ViewModels.RouteStations;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Queries;
public class GetTripByUserLocation : IRequest<TripCoordinateResponseModel?>
{
    public Guid RouteVarId { get; set; } = Guid.Empty;
    public decimal Latitude { get; set; } = 0;
    public decimal Longitude { get; set; } = 0;
    public class QueryHandler : IRequestHandler<GetTripByUserLocation, TripCoordinateResponseModel?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RouteStationBusinesses routeStationBusinesses;
        private readonly ILocationService locationService;
        private readonly IMediator mediator;
        public QueryHandler(IUnitOfWork unitOfWork, ILocationService locationService,
            IMediator mediator)
        {
            this.mediator = mediator;
            this.locationService = locationService;
            this.routeStationBusinesses = new(unitOfWork);
            this.unitOfWork = unitOfWork;
        }
        public async Task<TripCoordinateResponseModel?> Handle(GetTripByUserLocation request, CancellationToken cancellationToken)
        {
            var timeTable = (await unitOfWork.TimeTableRepository.WhereAsync(x => x.RouteVarId == request.RouteVarId && x.IsCurrent)
                ?? throw new Exception("Not found timetable")).First();

            var currentCoordinate = new { request.Latitude, request.Longitude };

            var routeStations = await routeStationBusinesses.GetRouteStationByRouteVarId(request.RouteVarId);
            var routeVar = await unitOfWork.RouteVarRepository.GetByIdAsync(request.RouteVarId, x => x.Route);
            var trips = await unitOfWork.TripRepository.WhereAsync(x => x.TimeTableId == timeTable.Id);
            var now = DateTime.UtcNow;
            System.Console.WriteLine(now);
            if (routeVar?.Route?.AverageVelocity <= 0)
            {
                throw new Exception($"RouteVaration: {routeVar.RouteVarName} chưa support tính duration! add endpoint 'distance-modification'");
            }
            if (routeStations.Count > 0 && trips.Count > 0 && routeVar?.Route?.AverageVelocity > 0)
            {
                // ToDo
                var points = RouteStationViewModel.To(routeStations);
                var upperIndex = FindUpperBoundIndex(points, currentCoordinate.Latitude, currentCoordinate.Longitude);
                var lastStation = routeStations.MaxBy(x => x.Index);
                var distance = await locationService.GetDistance(orgLat: currentCoordinate.Latitude,
                    orgLng: currentCoordinate.Longitude,
                    destLat: lastStation!.Latitude,
                    destLng: lastStation!.Longitude);
                var duration = distance / routeVar.Route.AverageVelocity;


                var eta = DateTime.Now.AddMinutes(duration);

                var currentEstimateTrip = trips.MinBy(x => Math.Abs((eta - DateTime.Parse(x.EndTime)).TotalMilliseconds));
                
                var trip = await mediator.Send(new GetTripByIdQuery
                {
                    Id = currentEstimateTrip!.Id,
                    IsSchedule = true
                }, cancellationToken: cancellationToken);
                return new()
                {
                    Trip = trip,
                    Schedule = trip.Schedules.FirstOrDefault(x => x.Index == upperIndex) ?? new()
                };

            }
            else return null;


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