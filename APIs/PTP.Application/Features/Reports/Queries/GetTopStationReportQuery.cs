using System.Data.SqlTypes;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Reports;

namespace PTP.Application.Features.Reports.Queries;
public class GetTopStationReportQuery : IRequest<List<TopOrderStationModel>?>
{
    public class QueryHandler : IRequestHandler<GetTopStationReportQuery, List<TopOrderStationModel>?>
    {
        private readonly IUnitOfWork unitOfWork;
        private ILogger<GetTopStationReportQuery> logger;
        private readonly IConnectionConfiguration connection;

        public QueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetTopStationReportQuery> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.connection = unitOfWork.DirectionConnection;
        }
        public async Task<List<TopOrderStationModel>?> Handle(GetTopStationReportQuery request, CancellationToken cancellationToken)
        {
            string sql = SqlQueriesStorage.GET_TOP_ORDER_STATION;
            using var conn = connection.GetDbConnection();
            var executeResult = await conn.QueryAsync<TopOrderStationModel>(
                sql: sql,
                param: null,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text
            );
            logger.LogInformation(nameof(GetTopStationReportQuery), executeResult?.Count());
            return executeResult?.ToList();
        }
    }
}