using System.Data;
using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;

namespace PTP.Application.Features.RouteVars.Queries;

public class GetStoresByRouteVarId : IRequest<List<StoreViewModel>>
{
    public Guid RouteVarId { get; set; } = Guid.Empty;
    public class QueryHandler : IRequestHandler<GetStoresByRouteVarId, List<StoreViewModel>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConnectionConfiguration connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            connection = unitOfWork.DirectionConnection;
        }
        public async Task<List<StoreViewModel>> Handle(GetStoresByRouteVarId request, CancellationToken cancellationToken)
        {
            using var dbConnection = connection.GetDbConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@routeVarId", request.RouteVarId);
            var executeResult = await dbConnection.QueryAsync<Store>(
                sql: SqlQueriesStorage.GET_STORES_BY_ROUTEVARID,
                param: parameters,
                transaction: null,
                commandTimeout: 30,
                commandType: CommandType.Text
            );
            return unitOfWork.Mapper.Map<List<StoreViewModel>>(executeResult);
        }
    }
}
