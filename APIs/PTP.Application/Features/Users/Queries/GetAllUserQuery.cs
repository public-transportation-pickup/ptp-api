using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Users;

namespace PTP.Application.Features.Users.Queries;
public class GetAllUserQuery : IRequest<IEnumerable<UserViewModel>?>
{
    public class QueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserViewModel>?>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<IEnumerable<UserViewModel>?> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_ALL_USER; 
            var result = await connection.QueryAsync<UserViewModel>(query) ?? new List<UserViewModel>();
            return result;
        }
    }
}