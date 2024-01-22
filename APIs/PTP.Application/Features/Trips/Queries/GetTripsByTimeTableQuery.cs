using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Queries;
public class GetTripsByTimeTableIdQuery : IRequest<IEnumerable<TripViewModel>>
{
    public Guid TimeTableId { get; set; }
    public class QueryHandler : IRequestHandler<GetTripsByTimeTableIdQuery, IEnumerable<TripViewModel>>
    {
        public IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<IEnumerable<TripViewModel>> Handle(GetTripsByTimeTableIdQuery request, CancellationToken cancellationToken)
        {
             using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_TRIP_BY_TIMETABLEID;
            var parameters = new DynamicParameters();
            parameters.Add("@timeTableId", request.TimeTableId);
            
            var resultFromDb = await connection.QueryAsync<TripViewModel>(query, parameters);
            if (resultFromDb?.Count() > 0)
            {
                return resultFromDb;
            }
            else throw new NotFoundException($"no_data_found in {nameof(GetAllTripByRouteIdQuery)}");
        }
    }
}