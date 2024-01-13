using System.Diagnostics.CodeAnalysis;
using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Queries;
public class GetAllTripByRouteIdQuery : IRequest<IEnumerable<TripViewModel>>
{
    public Guid RouteId { get; set; }
    public Guid RouteVarId { get; set; }
    public class QueryHandler : IRequestHandler<GetAllTripByRouteIdQuery, IEnumerable<TripViewModel>>
    {
        private readonly IConnectionConfiguration connectionConfiguration;
        public QueryHandler(IConnectionConfiguration connectionConfiguration)
        {
            this.connectionConfiguration = connectionConfiguration;
        }
        public async Task<IEnumerable<TripViewModel>> Handle(GetAllTripByRouteIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionConfiguration.GetDbConnection();
            var query = SqlQueriesStorage.GET_TRIPS_BY_PARENTS_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.RouteId);
            parameters.Add("@routeVarId", request.RouteVarId);
            var resultFromDb = await connection.QueryAsync<TripViewModel>(query, parameters);
            if (resultFromDb?.Count() > 0)
            {
                return resultFromDb;
            }
            else throw new NotFoundException($"no_data_found in {nameof(GetAllTripByRouteIdQuery)}");
        }
    }
}