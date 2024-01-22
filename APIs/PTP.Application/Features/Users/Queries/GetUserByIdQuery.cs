using System.Data;
using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Users;

namespace PTP.Application.Features.Users.Queries;
public class GetUserByIdQuery : IRequest<UserViewModel>
{
    public Guid Id { get; set; } = Guid.Empty;
    public class QueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_USER_BY_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.Id);
            var result = await connection.QueryFirstOrDefaultAsync<UserViewModel>(
                sql: query,
                param: parameters,
                transaction: null,
                commandTimeout: 90,
                commandType: CommandType.Text)
                        ?? throw new Exception($"Error: {nameof(GetUserByIdQuery)}: no_data_found");
            return result;
        }
    }
}