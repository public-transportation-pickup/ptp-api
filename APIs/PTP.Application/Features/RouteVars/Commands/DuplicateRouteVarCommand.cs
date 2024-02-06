using FluentValidation;
using MediatR;
using PTP.Application.Features.Routes.Commands;
using PTP.Application.Features.RouteStations.Queries;
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
		private readonly IMediator mediator;
		public CommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
		{
			this.unitOfWork = unitOfWork;

			this.mediator = mediator;
		}
		public async Task<IEnumerable<RouteStationViewModel>> Handle(DuplicateRouteVarCommand request, CancellationToken cancellationToken)
		{

			var routeVariation = await unitOfWork.RouteVarRepository.GetByIdAsync(request.Id)
							?? throw new Exception($"Error: {nameof(DuplicateRouteVarCommand)}-no_data_found");
			var oldRouteStations = await unitOfWork.RouteStationRepository.WhereAsync(x => x.RouteVarId == request.Id);
			if (routeVariation.RouteStations?.Count <= 0) throw new Exception();
			unitOfWork.RouteVarRepository.SoftRemove(routeVariation);
			unitOfWork.RouteStationRepository.SoftRemoveRange(oldRouteStations);
			var newRouteVar = new RouteVar
			{
				RouteId = routeVariation.RouteId,
				EndStop = routeVariation.EndStop,
				StartStop = routeVariation.StartStop,
				IsDistance = false,
				RouteVarId = routeVariation.RouteVarId,
				OutBound = routeVariation.OutBound,
				RouteVarName = routeVariation.RouteVarName,
				RouteVarShortName = routeVariation.RouteVarShortName,
				Timetables = routeVariation.Timetables,
				RunningTime = routeVariation.RunningTime,

			};



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
			await unitOfWork.SaveChangesAsync();
			//var distanceModifiedTask = await mediator.Send(new DistanceModificationCommand { Id = routeVariation.RouteId }, cancellationToken);
			var getRouteStationTask = await mediator.Send(new GetRouteStationByParIdQuery { Id = newRouteVar.RouteId, RouteVarId = newRouteVar.Id });
			return getRouteStationTask;

		}


	}
}