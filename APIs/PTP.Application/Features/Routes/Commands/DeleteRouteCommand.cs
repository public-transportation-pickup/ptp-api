using MediatR;
using PTP.Application.GlobalExceptionHandling.Exceptions;

namespace PTP.Application.Features.Routes.Commands;
public class DeleteRouteCommand : IRequest<bool>
{
    public Guid Id { get; set; }


    public class CommandHandler : IRequestHandler<DeleteRouteCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
        {
            var routeTask = unitOfWork.RouteRepository.GetByIdAsync(request.Id);
            var routeVarTask = unitOfWork.RouteVarRepository.WhereAsync(x => x.RouteId == request.Id);
            var timetableTask = unitOfWork.TimeTableRepository.WhereAsync(x => x.RouteId == request.Id);
            var routeStationTask = unitOfWork.RouteStationRepository.WhereAsync(x => x.RouteId == request.Id);
            await Task.WhenAll(routeTask, routeVarTask, timetableTask, routeStationTask);
            unitOfWork.RouteRepository.SoftRemove(routeTask.Result ?? throw new Exception($"no_route_found at {nameof(DeleteRouteCommand)}"));
            unitOfWork.RouteVarRepository.SoftRemoveRange(routeVarTask.Result);
            unitOfWork.TimeTableRepository.SoftRemoveRange(timetableTask.Result);
            unitOfWork.RouteStationRepository.SoftRemoveRange(routeStationTask.Result);
            return await unitOfWork.SaveChangesAsync();            
        }
    }
}