using System.Diagnostics.CodeAnalysis;
using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Queries;
public class GetAllTripByRouteIdQuery : IRequest<PaginatedList<TripViewModel>>
{
    public Dictionary<string, string> Filter { get; set; } = default!;
    public Guid RouteId { get; set; }
    public Guid RouteVarId { get; set; }
    public int PageNumber { get; set; } = -1;
    public class QueryHandler : IRequestHandler<GetAllTripByRouteIdQuery, PaginatedList<TripViewModel>>
    {
        private readonly IConnectionConfiguration connectionConfiguration;
        public QueryHandler(IConnectionConfiguration connectionConfiguration)
        {
            this.connectionConfiguration = connectionConfiguration;
        }
        public async Task<PaginatedList<TripViewModel>> Handle(GetAllTripByRouteIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Filter?.Count > 0)
            {
                request.Filter.Remove("routeId");
                request.Filter.Remove("routeVarId");
                request.Filter.Remove("pageNumber");
            }
            using var connection = connectionConfiguration.GetDbConnection();
            var query = SqlQueriesStorage.GET_TRIPS_BY_PARENTS_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.RouteId);
            parameters.Add("@routeVarId", request.RouteVarId);
            var resultFromDb = await connection.QueryAsync<TripViewModel>(query, parameters);
            var returnResult = new List<TripViewModel>();
            if (request.Filter?.Count > 0)
            {
                foreach (var item in request.Filter)
                {
                    // TODO, Loop to find matching 
                    //System.Console.WriteLine(FilterUtilities.SelectItems(result, item.Key, item.Value).ToList().Count);
                    returnResult = returnResult.Union(FilterUtilities.SelectItems(resultFromDb, item.Key, item.Value)).ToList();

                }
            } else returnResult = resultFromDb.ToList();

            return PaginatedList<TripViewModel>.Create(
                source: returnResult.AsQueryable(),
                pageIndex: request.PageNumber >= 0 ? 0 : request.PageNumber,
                pageSize: request.PageNumber >=0 ? 20 : returnResult.Count
            );


        }
    }
}