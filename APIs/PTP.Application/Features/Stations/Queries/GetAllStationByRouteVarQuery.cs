using Dapper;
using FluentValidation;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.ViewModels.Stations;

namespace PTP.Application.Features.Stations.Queries;
public class GetAllStationByRouteIdQuery : IRequest<List<StationShortViewModel>>
{
    public Guid Id { get; set; } = Guid.Empty;
    public class QueryValidation : AbstractValidator<GetAllStationByRouteIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class QueryHandler : IRequestHandler<GetAllStationByRouteIdQuery, List<StationShortViewModel>>
    {
        private readonly IUnitOfWork unitOfWork;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<StationShortViewModel>> Handle(GetAllStationByRouteIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = unitOfWork.DirectionConnection.GetDbConnection();
            DynamicParameters parameters = new();
            parameters.Add("@routeVarId", request.Id);
            var executeResult = await connection.QueryAsync<StationShortViewModel>(
                sql: SqlQueriesStorage.GET_STATION_BY_ROUTEVARID,
                param: parameters,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text
            );
            return executeResult?.Count() > 0 
            ? executeResult.ToList() 
            : throw new Exception($"Error {nameof(GetAllStationByRouteIdQuery)}-no_data_found");
        }
    }
}