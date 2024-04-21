using MediatR;
using PTP.Application.ViewModels.Timetables;

namespace PTP.Application.Features.Timetables.Queries;
public class GetTimeTableByDateQuery : IRequest<TimetableViewModel?>
{
    public class QueryHandler : IRequestHandler<GetTimeTableByDateQuery, TimetableViewModel?>
    {
        private readonly IUnitOfWork 
    }
}