using Dapper;
using MediatR;


using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Routes;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Routes.Queries;
public class GetRouteByStationQuery : IRequest<PaginatedList<RouteViewModel>>
{
    public string StationName { get; set; } = default!;
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public class QueryHandler : IRequestHandler<GetRouteByStationQuery, PaginatedList<RouteViewModel>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConnectionConfiguration connection;
        private readonly ILogger<GetRouteByStationQuery> logger;
        public QueryHandler(IUnitOfWork uniOfWork,
            ILogger<GetRouteByStationQuery> logger)
        {
            unitOfWork = uniOfWork;
            this.logger = logger;
            connection = uniOfWork.DirectionConnection;
        }

        async Task<PaginatedList<RouteViewModel>> IRequestHandler<GetRouteByStationQuery, PaginatedList<RouteViewModel>>.Handle(GetRouteByStationQuery request, CancellationToken cancellationToken)
        {
            using var dbConnection = connection.GetDbConnection();
            var input = new { request.StationName, request.PageNumber, request.PageSize };
            logger.LogInformation("input", input);
            DynamicParameters parameters = new();
            parameters.Add("@stationName", $"%{input.StationName}%");
            var executeResults = await dbConnection.QueryAsync<Route>(
                sql: SqlQueriesStorage.GET_ROUTE_BY_STATION_NAME,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text,
                param: parameters
            );
            var mapperResult = unitOfWork.Mapper.Map<IEnumerable<RouteViewModel>>(executeResults);
            return PaginatedList<RouteViewModel>.Create(
                mapperResult.AsQueryable(),
                pageIndex: request.PageNumber ?? 0,
                pageSize: request.PageSize ?? mapperResult.Count()
            );



        }
    }
}