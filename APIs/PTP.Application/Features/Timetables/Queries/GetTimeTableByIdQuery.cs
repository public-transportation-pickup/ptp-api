using System.Data;
using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Timetables;

namespace PTP.Application.Features.Timetables.Queries;
public class GetTimeTableByIdQuery : IRequest<TimetableViewModel?>
{
    public Guid Id { get; set; }
    public class QueryHandler : IRequestHandler<GetTimeTableByIdQuery, TimetableViewModel?>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<TimetableViewModel?> Handle(GetTimeTableByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_TIMETABLE_BY_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.Id);
            var result = await connection.QueryFirstOrDefaultAsync<TimetableViewModel>(
                sql: query,
                param: parameters,
                transaction: null,
                commandTimeout: 90,
                commandType: CommandType.Text);
            return result;
        }
    }
}