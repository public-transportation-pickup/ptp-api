using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Users;

namespace PTP.Application.Features.Users.Queries;
public class GetAllUserQueryModel
{
    public Dictionary<string, string> Filter { get; set; } = default!;
    public int PageNumber { get; set; } = -1;
}
public class GetAllUserQuery : GetAllUserQueryModel, IRequest<PaginatedList<UserViewModel>?>
{
    public class QueryHandler : IRequestHandler<GetAllUserQuery, PaginatedList<UserViewModel>?>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<PaginatedList<UserViewModel>?> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            if(request.Filter.Count > 0)
            {
                request.Filter.Remove("pageNumber");
            }
            using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_ALL_USER;
            var result = await connection.QueryAsync<UserViewModel>(query) ?? new List<UserViewModel>();
            // Adding Fillter
            var returnResult = new List<UserViewModel>();
            System.Console.WriteLine(request.Filter?.Count);
            if (request.Filter?.Count > 0)
            {
               
                //request.Filter.Remove("PageNumber");
                foreach (var item in request.Filter)
                {
                    returnResult = returnResult.Union(FilterUtilities.SelectItems(result, item.Key, item.Value)).ToList();
                }
            }
            else returnResult = result.ToList();
            return PaginatedList<UserViewModel>.Create(
                source: returnResult.AsQueryable(),
                pageIndex: request.PageNumber >= 0 ? request.PageNumber : 0,
                pageSize: request.PageNumber >= 0 ? 100 : returnResult.Count
            );
        }
    }
}