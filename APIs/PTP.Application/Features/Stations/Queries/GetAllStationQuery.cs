using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Stations;

namespace PTP.Application.Features.Stations.Queries;
public class GetAllStationQuery : IRequest<PaginatedList<StationViewModel>>
{
    public Dictionary<string, string> Filter { get; set; } = new();
    public int PageNumber { get; set; } = -1;
    public int PageSize { get; set; } = 100;
    public class QueryHandler : IRequestHandler<GetAllStationQuery, PaginatedList<StationViewModel>>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<PaginatedList<StationViewModel>> Handle(GetAllStationQuery request, CancellationToken cancellationToken)
        {
            var query = "SELECT * FROM STATION WHERE [IsDeleted] = 0 ORDER BY StopId";
            request.Filter.Remove("pageNumber");
            using var connection = _connection.GetDbConnection();
            var result = await connection.QueryAsync<StationViewModel>(
                sql: query,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text
            );
            var filterResult = request.Filter.Count > 0 ? new List<StationViewModel>() : result;
            if (request.Filter?.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResult = filterResult.Union(FilterUtilities.SelectItems(result, filter.Key, filter.Value));
                }
            }

            return PaginatedList<StationViewModel>.Create(
                source: filterResult.AsQueryable(),
                pageIndex: request.PageNumber >= 0 ? 0 : request.PageNumber,
                pageSize: request.PageNumber >= 0 ? request.PageSize : filterResult.Count()
            );
        }
    }
}