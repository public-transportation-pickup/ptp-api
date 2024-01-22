using Dapper;
using MediatR;
using Microsoft.Identity.Client;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Timetables;

namespace PTP.Application.Features.Timetables.Queries;
public class GetTimetableByParIdQuery : IRequest<IEnumerable<TimetableViewModel>?>
{
    public Guid RouteId { get; set; }
    public Guid RouteVarId { get; set; }
    public class QueryHandler : IRequestHandler<GetTimetableByParIdQuery, IEnumerable<TimetableViewModel>?>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public async Task<IEnumerable<TimetableViewModel>?> Handle(GetTimetableByParIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var query = SqlQueriesStorage.GET_TIMETABLE_BY_PARID;
            var parameters = new DynamicParameters();
            parameters.Add("@routeId", request.RouteId);
            parameters.Add("@routeVarId", request.RouteVarId);
            var result = await  connection.QueryAsync<TimetableViewModel>
                        (query, param: parameters, null, 90, commandType: System.Data.CommandType.Text);
            if(result is not null && result.Any()) return result; 
            else return null;
        }
    }
}