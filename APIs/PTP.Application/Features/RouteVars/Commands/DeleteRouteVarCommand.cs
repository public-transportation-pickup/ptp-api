using MediatR;

namespace PTP.Application.Features.RouteVars.Commands;
public class DeleteRouteVarCommand : IRequest<bool>
{
    public Guid Id { get; set; } = default;
    public class CommandHandler : IRequestHandler<DeleteRouteVarCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteRouteVarCommand request, CancellationToken cancellationToken)
        {
            var routeVar = await _unitOfWork.RouteVarRepository.GetByIdAsync(request.Id, x => x.RouteStations, x => x.Timetables)
                ?? throw new NullReferenceException($"no_data_found at {nameof(DeleteRouteVarCommand)}"); 
            if(routeVar.Timetables?.Count > 0)
            {
                foreach(var timetable in routeVar.Timetables)
                {
                    var trips = await _unitOfWork.TripRepository.WhereAsync(x => x.TimeTableId == timetable.Id);
                    if(trips is not null && trips.Count > 0)
                    _unitOfWork.TripRepository.SoftRemoveRange(trips);
                }
                _unitOfWork.TimeTableRepository.SoftRemoveRange(routeVar.Timetables.ToList());
            }
            if(routeVar.RouteStations.Any())
            {
                _unitOfWork.RouteStationRepository.SoftRemoveRange(routeVar.RouteStations.ToList());
            }

            return await _unitOfWork.SaveChangesAsync();            
        }
    }
}