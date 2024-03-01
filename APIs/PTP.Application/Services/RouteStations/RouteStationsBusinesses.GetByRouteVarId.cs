using Dapper;
using PTP.Application.Commons;
using PTP.Application.ViewModels.RouteStations;

namespace PTP.Application.Services.RouteStations;
public partial class RouteStationBusinesses
{
    public async Task<List<RouteStationViewModel>> GetRouteStationByRouteVarId(Guid routeVarId)
    {
        const string toolService = $"{nameof(RouteStationBusinesses)}_{nameof(GetRouteStationByRouteVarId)}";
        DynamicParameters parameters = new();
        parameters.Add("@routeVarId", routeVarId);
        var sql = SqlQueriesStorage.GET_ROUTE_STATION_BY_ROUTEVAR_ID;
        using var connection = unitOfWork.DirectionConnection.GetDbConnection();
        var result = await connection.QueryAsync<RouteStationViewModel>(sql: sql,
            param: parameters,
            transaction: null,
            commandTimeout: 30,
            commandType: System.Data.CommandType.Text);
        return result.ToList();
    }
}