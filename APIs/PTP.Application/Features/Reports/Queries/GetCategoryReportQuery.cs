using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Reports;

namespace PTP.Application.Features.Reports.Queries;
public class GetCategoryReportQuery : IRequest<List<CategoryReportViewModel>?>
{
    public class QueryHandler : IRequestHandler<GetCategoryReportQuery, List<CategoryReportViewModel>?>
    {
        private readonly ILogger<GetCategoryReportQuery> logger;
        private readonly IConnectionConfiguration connectionConfiguration;
        public QueryHandler(ILogger<GetCategoryReportQuery> logger,
            IUnitOfWork unitOfWork)
        {
            this.connectionConfiguration = unitOfWork.DirectionConnection;
            this.logger = logger;
        }

        public async Task<List<CategoryReportViewModel>?> Handle(GetCategoryReportQuery request, CancellationToken cancellationToken)
        {
            const string toolService = nameof(GetCategoryReportQuery);
            using var connection = connectionConfiguration.GetDbConnection();
            var result = await connection.QueryAsync<CategoryReportViewModel>(sql: SqlQueriesStorage.GET_CATEGORIES_REPORT,
                param: null,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text);
            logger.LogInformation($"{toolService}_Result: {result?.Count()}");
            return result?.ToList();

        }
    }
}