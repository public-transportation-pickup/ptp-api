// using Dapper;
// using MediatR;
// using Microsoft.Extensions.Logging;
// using PTP.Application.Commons;
// using PTP.Application.Data.Configuration;
// using PTP.Application.ViewModels.Reports;

// namespace PTP.Application.Features.Reports.Queries;
// public class GetTopProfitStoreQuery : IRequest<List<TopProfitStoreModel>?>
// {

//     public class QueryHandler : IRequestHandler<GetTopProfitStoreQuery, List<TopProfitStoreModel>?>
//     {
//         private readonly ILogger<GetTopProfitStoreQuery> logger;
//         private readonly IUnitOfWork unitOfWork;
//         private readonly IConnectionConfiguration connConfig;
//         public QueryHandler(ILogger<GetTopProfitStoreQuery> logger,
//             IUnitOfWork unitOfWork)
//         {
//             this.unitOfWork = unitOfWork;
//             this.logger = logger;
//             this.connConfig = unitOfWork.DirectionConnection;
//         }
//         public async Task<List<TopProfitStoreModel>?> Handle(GetTopProfitStoreQuery request, CancellationToken cancellationToken)
//         {
//             string sql = SqlQueriesStorage.GET_TOP_PROFIT_STORES;
//             using var connection = connConfig.GetDbConnection();
//             var executeResult = await connection.QueryAsync<TopProfitStoreModel>(sql: sql,
//                 param: null,
//                 transaction: null,
//                 commandTimeout: 30,
//                 commandType: null);
//             logger.LogInformation(nameof(GetTopProfitStoreQuery), executeResult?.Count());
//             return executeResult?.ToList();
//         }
//     }
// }