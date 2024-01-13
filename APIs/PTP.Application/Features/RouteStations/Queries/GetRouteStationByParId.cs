using Dapper;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.RouteStations;
using PTP.Application.ViewModels.RouteVars;

namespace PTP.Application.Features.RouteStations.Queries;
public class GetRouteStationByParIdQuery : IRequest<IEnumerable<RouteStationViewModel>>
{
    public Guid Id { get; set; } = default!;
    public Guid RouteVarId { get; set; } = default!;
    public class QueryHandler : IRequestHandler<GetRouteStationByParIdQuery, IEnumerable<RouteStationViewModel>>
    {
        private readonly IConnectionConfiguration connectionConfiguration;
        public QueryHandler(IConnectionConfiguration connectionConfiguration)
        {
            this.connectionConfiguration = connectionConfiguration;
        }
        public async Task<IEnumerable<RouteStationViewModel>> Handle(GetRouteStationByParIdQuery request, CancellationToken cancellationToken)
        {
            var query = SqlQueriesStorage.GET_ROUTE_STATION_BY_PARENT_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.Id);
            parameters.Add("@routeVarId", request.RouteVarId);
            
            var resultFromDb = 
            await connectionConfiguration.GetDbConnection().QueryAsync<RouteStationViewModel>(query, parameters, null, commandTimeout: 90, commandType: System.Data.CommandType.Text);
            if(resultFromDb?.Count() > 0)
            {
                return resultFromDb;
            } else throw new Exception("no_data_found");

        }
    }

}