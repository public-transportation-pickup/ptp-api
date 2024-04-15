using System.Data;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Stations;

namespace PTP.Application.Features.Stations.Queries;
public class GetStationRevenueQuery : IRequest<List<StationRevenueModel>?>
{
    public Dictionary<string, string>? Filter { get; set; } = new();
    public class QueryHandler : IRequestHandler<GetStationRevenueQuery, List<StationRevenueModel>?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetStationRevenueQuery> logger;
        public QueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetStationRevenueQuery> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<StationRevenueModel>?> Handle(GetStationRevenueQuery request, CancellationToken cancellationToken)
        {
            using var connection = unitOfWork.DirectionConnection.GetDbConnection();
            var sql = SqlQueriesStorage.GET_STATIONS_REVENUE;
            const string toolService = nameof(GetStationRevenueQuery);
            var result = (await connection.QueryAsync<StationRevenueModel>(
                sql: sql,
                null, null, 30, CommandType.Text
            ))?.ToList();

            logger.LogInformation($"source:{toolService}_Result: {result?.Count()}");
            if (result?.Count() <= 0)
                throw new Exception($"source: {toolService}_no_data_found");
            List<StationRevenueModel> returnResult = result ?? new();
            if (request.Filter?.Count > 0)
            {
               
                foreach (var item in request.Filter)
                {
                    System.Console.WriteLine(item.Key);
                    // TODO, Loop to find matching 
                    //System.Console.WriteLine(FilterUtilities.SelectItems(result, item.Key, item.Value).ToList().Count);

                    returnResult = returnResult!.Union(FilterUtilities.SelectItems(result ?? new(), item.Key, item.Value).ToList()).ToList();

                }
            }
           
            return returnResult;
        }
    }
}