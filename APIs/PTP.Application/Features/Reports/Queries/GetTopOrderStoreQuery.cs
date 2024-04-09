using System.Runtime.CompilerServices;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Reports;

namespace PTP.Application.Features.Reports.Queries;
public class GetTopOrderStoreQuery : IRequest<List<TopOrderStoreModel>?>
{
    public class QueryHandler : IRequestHandler<GetTopOrderStoreQuery, List<TopOrderStoreModel>?>
    {
        private readonly IConnectionConfiguration conn;
        private readonly ILogger<GetTopOrderStoreQuery> logger;
        public QueryHandler(ILogger<GetTopOrderStoreQuery> logger,
            IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.conn = unitOfWork.DirectionConnection;
        }
        public async Task<List<TopOrderStoreModel>?> Handle(GetTopOrderStoreQuery request, CancellationToken cancellationToken)
        {
            string sql = SqlQueriesStorage.GET_TOP_ORDER_STORES;
            using var connection = conn.GetDbConnection();
            var executeResult = await connection.QueryAsync<TopOrderStoreModel>(sql: sql, 
                param: null,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text);
            return executeResult?.ToList();

        }
    }
}