using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PTP.Application.Commons;
using PTP.Application.ViewModels.Reports;
using PTP.Domain.Entities;

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
        private List<decimal> GetSaleValueCurrent(List<Order> orders)
        {
            if (orders is null || orders.Count <= 0)
            {
                // No orders exist, return sale values as 0 for each day of the week
                return Enumerable.Range(0, 7).Select(_ => 0m).ToList();
            }

            DateTime today = DateTime.Today;
            DateTime mondayOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime sundayOfWeek = mondayOfWeek.AddDays(6);

            var saleValue = orders
                .Where(x => x.Status == "Completed" && x.CreationDate >= mondayOfWeek && x.CreationDate <= sundayOfWeek)
                .GroupBy(x => x.CreationDate.Day)
                .OrderBy(g => g.Key); // Ensure days are in chronological order

            List<decimal> saleValueCurrent = new List<decimal>();

            // Loop through each day of the week (from Monday to Sunday)
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = mondayOfWeek.AddDays(i);

                // Find orders for the current day (if any)
                var ordersForDay = saleValue.FirstOrDefault(g => g.Key == currentDate.Day);

                if (ordersForDay != null)
                {
                    // Sum the total values for orders on this day
                    decimal totalForDay = ordersForDay.Sum(x => x.Total);
                    saleValueCurrent.Add(totalForDay);
                }
                else
                {
                    // No orders found for this day, so sale value is 0
                    saleValueCurrent.Add(0m);
                }
            }

            return saleValueCurrent;
        }
        private List<decimal> GetSaleValueLast(List<Order> orders)
        {
            if (orders is null || orders.Count <= 0)
            {
                // No orders exist, return sale values as 0 for each day of the week
                return Enumerable.Range(0, 7).Select(_ => 0m).ToList();
            }

            DateTime today = DateTime.Today;
            DateTime mondayOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(-7);
            DateTime sundayOfWeek = mondayOfWeek.AddDays(6);

            var saleValue = orders
                .Where(x => x.Status == "Completed" && x.CreationDate >= mondayOfWeek && x.CreationDate <= sundayOfWeek)
                .GroupBy(x => x.CreationDate.Day)
                .OrderBy(g => g.Key); // Ensure days are in chronological order

            List<decimal> saleValueCurrent = new List<decimal>();

            // Loop through each day of the week (from Monday to Sunday)
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = mondayOfWeek.AddDays(i);

                // Find orders for the current day (if any)
                var ordersForDay = saleValue.FirstOrDefault(g => g.Key == currentDate.Day);

                if (ordersForDay != null)
                {
                    // Sum the total values for orders on this day
                    decimal totalForDay = ordersForDay.Sum(x => x.Total);
                    saleValueCurrent.Add(totalForDay);
                }
                else
                {
                    // No orders found for this day, so sale value is 0
                    saleValueCurrent.Add(0m);
                }
            }

            return saleValueCurrent;
        }
        public async Task<AdminReportViewModel> Handle(GetAdminReportQuery request, CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.OrderRepository.WhereAsync(x => x.Status == "Completed");

            var getTopOrderStoreTask = mediator.Send(new GetTopOrderStoreQuery());
            var getTopOrderStationTask = mediator.Send(new GetTopStationReportQuery());
            var getCateTask = mediator.Send(new GetCategoryReportQuery());
            using var connection = unitOfWork.DirectionConnection.GetDbConnection();
            var getSumRoutesTask = connection.QueryFirstOrDefaultAsync<AdminReportViewModel>(sql: SqlQueriesStorage.GET_SUM_ROUTES,
                param: null,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text);
            await Task.WhenAll(getTopOrderStationTask, getTopOrderStoreTask, getCateTask, getSumRoutesTask);
            var topOrderStore = getTopOrderStoreTask.Result;
            var categories = getCateTask.Result;
            var routes = getSumRoutesTask.Result ?? new();
            var topOrderStation = getTopOrderStationTask.Result;
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_TopOrderStation", topOrderStation?.Count);
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_TopOrderStore", topOrderStore?.Count);
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_Categories_{categories?.Count}");
            logger.LogInformation($"{nameof(GetAdminReportQuery)}_Routes_Stations", routes);

            return new()
            {
                Routes = routes.Routes,
                Stations = routes.Stations,
                Stores = routes.Stores,
                Revenue = routes.Revenue,
                TopOrderStations = topOrderStation ?? new(),
                TopOrderStores = topOrderStore ?? new(),
                Categories = categories ?? new(),
                SaleValueCurrent = GetSaleValueCurrent(orders: orders),
                SaleValueLast = GetSaleValueLast(orders)

            };


        }
    }
}