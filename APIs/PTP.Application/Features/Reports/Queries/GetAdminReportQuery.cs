using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.ViewModels.Reports;

namespace PTP.Application.Features.Reports.Queries;
public class GetAdminReportQuery : IRequest<AdminReportViewModel>
{
    public class QueryHandler : IRequestHandler<GetAdminReportQuery, AdminReportViewModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetAdminReportQuery> logger;
        private readonly IMediator mediator;
        public QueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetAdminReportQuery> logger,
            IMediator mediator)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<AdminReportViewModel> Handle(GetAdminReportQuery request, CancellationToken cancellationToken)
        {

            var getTopOrderStoreTask = mediator.Send(new GetTopOrderStoreQuery());
            var getTopOrderStationTask = mediator.Send(new GetTopStationReportQuery());
            var getTopProfitStoreTask = mediator.Send(new GetTopProfitStoreQuery());
            using var connection = unitOfWork.DirectionConnection.GetDbConnection();
            var getSumRoutesTask = connection.QueryFirstOrDefaultAsync<AdminReportViewModel>(sql: SqlQueriesStorage.GET_SUM_ROUTES,
                param: null,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text);
            await Task.WhenAll(getTopOrderStationTask, getTopOrderStoreTask, getTopProfitStoreTask, getSumRoutesTask);
            var topOrderStore = getTopOrderStoreTask.Result;
            var topProfitStore = getTopProfitStoreTask.Result;
            var routes = getSumRoutesTask.Result ?? new();
            var topOrderStation = getTopOrderStationTask.Result;
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_TopOrderStation", topOrderStation?.Count);
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_TopOrderStore", topOrderStore?.Count);
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_TopProfitStore", topProfitStore?.Count);
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_Routes_Statiosn", routes);

            return new()
            {
                Routes = routes.Routes,
                Stations = routes.Stations,
                TopOrderStations = topOrderStation ?? new(),
                TopOrderStores = topOrderStore ?? new(),
                TopProfitStores = topProfitStore ?? new()
            };


        }
    }
}