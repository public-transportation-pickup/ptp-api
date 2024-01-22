using FluentValidation;
using MediatR;
using PTP.Application.ViewModels.Trips;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Trips.Commands;
public class CreateTripCommand : IRequest<TripViewModel?>
{
    public TripCreateModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<CreateTripCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Model.StartTime)
            .NotNull()
            .NotEmpty()
            .Matches(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage($"{nameof(CreateTripCommand)}: StartTime is required and in format: 'HH:MM'");
            
        }
    }
    public class CommandHandler : IRequestHandler<CreateTripCommand, TripViewModel?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TripViewModel?> Handle(CreateTripCommand request, CancellationToken cancellationToken)
        {
            var timeTable = await _unitOfWork.TimeTableRepository.GetByIdAsync(request.Model.TimeTableId, x => x.Trips);
            
            if (timeTable is null)
            {
                throw new Exception($"Error: {nameof(CreateTripCommand)}: Create Trip failed! Timetable is not exist!");
            }
            else if (timeTable.Trips?.Count > 0)
            {
                if (timeTable.Trips.FirstOrDefault(x => x.IsDeleted == false) is not null)
                    throw new Exception($"Error: {nameof(CreateTripCommand)}: Create Trip failed! One trip is actived already!");
            }
            var trip = _unitOfWork.Mapper.Map<Trip>(request.Model);
            var routeVar = await _unitOfWork.RouteVarRepository.GetByIdAsync(timeTable.RouteVarId) ??
                throw new Exception($"Error: {nameof(CreateTripCommand)}_CanNotFindRouteVar");
            trip.EndTime = TimeSpan.Parse(trip.StartTime).Add(TimeSpan.FromMinutes(routeVar.RunningTime)).ToString();
            
            
            await _unitOfWork.TripRepository.AddAsync(trip);
            return await _unitOfWork.SaveChangesAsync()
            ? _unitOfWork.Mapper.Map<TripViewModel>(await _unitOfWork.TripRepository.GetByIdAsync(trip.Id))
            : throw new Exception($"Error: {nameof(CreateTripCommand)}: Create trip failed!");

        }
    }
}