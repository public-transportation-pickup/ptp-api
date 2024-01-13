using FluentValidation;
using MediatR;
using PTP.Application.IntergrationServices.Interfaces;

namespace PTP.Application.Features.Routes.Commands;
public class DistanceModificationCommand : IRequest<bool>
{
	public Guid Id { get; set; } = default!;
	public class CommmandValidation : AbstractValidator<DistanceModificationCommand>
	{
		public CommmandValidation()
		{
			RuleFor(x => x.Id).NotNull().NotEmpty();
		}
	}
	public class CommandHandler : IRequestHandler<DistanceModificationCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILocationService _locationService;
		public CommandHandler(IUnitOfWork unitOfWork, ILocationService locationService)
		{
			_locationService = locationService;
			_unitOfWork = unitOfWork;
		}
		public async Task<bool> Handle(DistanceModificationCommand request, CancellationToken cancellationToken)
		{
			var routeStations = (await _unitOfWork.RouteStationRepository.WhereAsync(x => x.RouteId == request.Id, x => x.Station))
																.OrderBy(x => x.Index)
																.GroupBy(x => x.RouteVarId)
																.ToList();
			var route = await _unitOfWork.RouteRepository.GetByIdAsync(request.Id);
			foreach (var routeVar in routeStations)
			{
				var rV = await _unitOfWork.RouteVarRepository.GetByIdAsync(routeVar.First().RouteVarId) ?? throw new Exception("no_data_found");
				var start = routeVar.First(x => x.Index == 0);
				foreach (var routeStation in routeVar)
				{
					var next = routeVar.FirstOrDefault(x => x.Index == routeStation.Index + 1);
					if (next is null) // LastIndex
					{
						routeStation.DistanceFromStart = await _locationService.GetDistance(
														routeStation.Station!.Latitude, routeStation.Station!.Longitude,
														start.Station!.Latitude, start.Station.Longitude
													);
						rV.Distance = routeStation.DistanceFromStart;
						routeStation.DistanceToNext = 0;
					}
					else if (routeStation.Index == 0) // Case is start
					{
						routeStation.DistanceFromStart = 0;
						routeStation.DistanceToNext = await _locationService.GetDistance(
														routeStation.Station!.Latitude, routeStation.Station!.Longitude,
														next!.Station.Latitude, next.Station.Longitude
													);
					}
					else // Case default
					{
						var taskList = new List<Task>();
						var getDistanceFromStart = _locationService.GetDistance(
														routeStation.Station!.Latitude, routeStation.Station!.Longitude,
														start.Station!.Latitude, start.Station.Longitude
													);
						var getDistanceToNext = _locationService.GetDistance(
															routeStation.Station!.Latitude, routeStation.Station!.Longitude,
															next!.Station.Latitude, next.Station.Longitude
														);
						await Task.WhenAll(getDistanceFromStart, getDistanceToNext);
						routeStation.DistanceFromStart = getDistanceFromStart.Result;
						routeStation.DistanceToNext = getDistanceToNext.Result;
					}

					routeStation.DurationFromStart = route!.AverageVelocity > 0 ? routeStation.DistanceFromStart / route!.AverageVelocity : 0;
					routeStation.DurationToNext = route.AverageVelocity > 0 ? routeStation.DistanceToNext / route!.AverageVelocity : 0;

				}
				routeVar.ToList().ForEach(x => x.Station = null!);
				_unitOfWork.RouteStationRepository.UpdateRange(routeVar.ToList());


				_unitOfWork.RouteVarRepository.Update(rV);
			}

			return await _unitOfWork.SaveChangesAsync();
		}
	}
}