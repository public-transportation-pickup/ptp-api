using System.Data;
using Dapper;
using MediatR;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.RouteVars;

namespace PTP.Application.Features.RouteVars.Queries;
public class GetRouteVarByIdQuery : IRequest<RouteVarViewModel?>
{
    public Guid Id { get; set; } = Guid.Empty;
    public class QueryHandler : IRequestHandler<GetRouteVarByIdQuery, RouteVarViewModel?>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<RouteVarViewModel?> Handle(GetRouteVarByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.Id);
            var query = "SELECT * FROM [RouteVars] WHERE Id = @id";
            var result = await connection
                        .QueryFirstOrDefaultAsync<RouteVarViewModel>(query, param: parameters, null, 90, CommandType.Text);
            return result;

        }
    }
}