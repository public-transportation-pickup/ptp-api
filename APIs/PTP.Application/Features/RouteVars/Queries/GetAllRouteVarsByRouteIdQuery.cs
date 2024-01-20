using System.Data;
using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.RouteVars;

namespace PTP.Application.Features.RouteVars.Queries;
public class GetAllRouteVarByRouteIdQuery : IRequest<IEnumerable<RouteVarViewModel>>
{
    public Guid RouteId { get; set; }
    public class QueryHandler : IRequestHandler<GetAllRouteVarByRouteIdQuery, IEnumerable<RouteVarViewModel>>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<IEnumerable<RouteVarViewModel>> Handle(GetAllRouteVarByRouteIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_ROUTE_VARS_BY_ROUTE_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@routeId", request.RouteId);
            var result = await connection.QueryAsync<RouteVarViewModel>(
                sql: query,
                param: parameters,
                null,
                commandTimeout: 90,
                CommandType.Text);
            return result;
        }
    }
}