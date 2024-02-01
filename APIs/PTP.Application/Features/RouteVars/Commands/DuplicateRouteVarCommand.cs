using FluentValidation;
using Hangfire;
using MediatR;
using PTP.Application.ViewModels.RouteStations;
using PTP.Domain.Entities;

namespace PTP.Application.Features.RouteVars.Commands;
public class DuplicateRouteVarCommand : IRequest<IEnumerable<RouteStationViewModel>>
{
    public Guid Id { get; set; } = Guid.Empty;
    public List<RouteStationDuplicateModel> Stations { get; set; } = new();
    public class CommandValidation : AbstractValidator<DuplicateRouteVarCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Stations).NotEmpty();
        }
    }

    public class CommandHandler : IRequestHandler<DuplicateRouteVarCommand, IEnumerable<RouteStationViewModel>>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RouteStationViewModel>> Handle(DuplicateRouteVarCommand request, CancellationToken cancellationToken)
        {
            var routeVariation = await unitOfWork.RouteVarRepository.GetByIdAsync(request.Id, x => x.RouteStations) 
                            ?? throw new Exception($"Error: {nameof(DuplicateRouteVarCommand)}-no_data_found");
            
            if(routeVariation.RouteStations?.Count <= 0) throw new Exception();
            unitOfWork.RouteVarRepository.SoftRemove(routeVariation);
            unitOfWork.RouteStationRepository.SoftRemoveRange(routeVariation.RouteStations?.ToList() ?? throw new Exception("Route Station is null"));
            var newRouteVar = unitOfWork.Mapper.Map<RouteVar>(routeVariation);
            newRouteVar.Id = Guid.NewGuid();
            await unitOfWork.RouteVarRepository.AddAsync(newRouteVar);
            await unitOfWork.SaveChangesAsync();
            var newRouteStations = request.Stations.Select(x => new RouteStation
            {
                Id = Guid.NewGuid(),
                StationId = x.StationId,
                Index = x.Index,
                RouteVarId = newRouteVar.Id,
                RouteId = routeVariation.RouteId,
            }).ToList();
            await unitOfWork.RouteStationRepository.AddRangeAsync(newRouteStations);

            
            
            return await unitOfWork.SaveChangesAsync() 
                    ? GetRouteStations()
                    : throw new Exception(""); 
        }

        private List<RouteStationViewModel> GetRouteStations() => new();
    }
}