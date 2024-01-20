using FluentValidation;
using MediatR;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Commands;
public class UpdateTripCommand : IRequest<bool>
{
	public Guid Id { get; set; } = default!;
	public TripUpdateModel Model { get; set; } = default!;
	public class CommandValidation : AbstractValidator<UpdateTripCommand>
	{
		public CommandValidation()
		{
			RuleFor(x => x.Model.StartTime)
		   .NotNull()
		   .NotEmpty()
		   .Matches(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")
		   .WithMessage($"{nameof(UpdateTripCommand)}: StartTime is required and in format: 'HH:MM'");
		}
	}
	public class CommandHandler : IRequestHandler<UpdateTripCommand, bool>
	{
		private readonly IUnitOfWork unitOfWork;
		public CommandHandler(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		public async Task<bool> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
		{
			var trip = await unitOfWork.TripRepository.GetByIdAsync(request.Id, x => x.TimeTable);
			if (trip is not null)
			{
				unitOfWork.Mapper.Map(request.Model, trip);

				var routeVar = await unitOfWork.RouteVarRepository.GetByIdAsync(trip.TimeTable.RouteVarId) ??
							throw new Exception($"Error: {nameof(CreateTripCommand)}_CanNotFindRouteVar");
				trip.EndTime = TimeSpan.Parse(trip.StartTime).Add(TimeSpan.FromMinutes(routeVar.RunningTime)).ToString();
				unitOfWork.TripRepository.Update(trip);
				return await unitOfWork.SaveChangesAsync();
			}
			else throw new
			NotFoundException($"no_data_found at {nameof(UpdateTripCommand)}");
		}
	}
}