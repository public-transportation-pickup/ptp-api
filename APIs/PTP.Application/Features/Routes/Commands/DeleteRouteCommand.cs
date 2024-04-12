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
            var routeTask = await unitOfWork.RouteRepository.GetByIdAsync(request.Id);
            var routeVarTask = await unitOfWork.RouteVarRepository.WhereAsync(x => x.RouteId == request.Id);
            var timetableTask = await unitOfWork.TimeTableRepository.WhereAsync(x => x.RouteId == request.Id);
            var routeStationTask = await unitOfWork.RouteStationRepository.WhereAsync(x => x.RouteId == request.Id);
   
            unitOfWork.RouteRepository.SoftRemove(routeTask ?? throw new Exception($"no_route_found at {nameof(DeleteRouteCommand)}"));
            unitOfWork.RouteVarRepository.SoftRemoveRange(routeVarTask);
            unitOfWork.TimeTableRepository.SoftRemoveRange(timetableTask);
            unitOfWork.RouteStationRepository.SoftRemoveRange(routeStationTask);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}