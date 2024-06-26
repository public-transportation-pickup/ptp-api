using System.Security;
using MediatR;
using Microsoft.Identity.Client.Extensibility;
using PTP.Application.Features.Trips.Queries;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Trips;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Timetables.Commands;
public class ApplyTripCommand : IRequest<IEnumerable<TripViewModel>>
{
    public Guid Id { get; set; } = Guid.Empty;
    public class CommandHandler : IRequestHandler<ApplyTripCommand, IEnumerable<TripViewModel>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        public CommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }
        public async Task<IEnumerable<TripViewModel>> Handle(ApplyTripCommand request, CancellationToken cancellationToken)
        {
            var timetable = await unitOfWork.TimeTableRepository.GetByIdAsync(request.Id, x => x.Route, x => x.RouteVar, x=> x.Trips);
            if(timetable?.Trips?.Count > 0)
            {
                throw new BadRequestException("Đã apply trip cho timetable này"); 
            }
            if (timetable is not null && timetable?.Route is not null)
            {
                var startEndTime = timetable.Route.OperationTime.ConvertToTimeSpanList();
                var timeSpacing = int.Parse(timetable.Route.HeadWay);
                var timeOfTrip = timetable.Route.TimeOfTrip.Length > 2
                    ? timetable.Route.TimeOfTrip.ConvertAverageTime()
                    : int.Parse(timetable.Route.TimeOfTrip);
                var startTime = startEndTime.Min();
                List<Trip> trips = new();
                var quantity = int.Parse(timetable.Route.TotalTrip);
                for (int i = 0; i < quantity; i++)
                {
                    try
                    {
                        var endTime = startTime.Add(TimeSpan.FromMinutes(timeOfTrip));
                        if (endTime > startEndTime.Max())
                        {
                            break;
                        }
                        trips.Add(new Trip
                        {
                            Id = Guid.NewGuid(),
                            Description = string.Empty,
                            StartTime = startTime.ToString("hh':'mm"),
                            EndTime = endTime.ToString("hh':'mm"),
                            TimeTableId = timetable.Id,
                            Name = string.Empty,
                            Status = DefaultStatusEnum.Active.ToString(),
                        });

                        startTime = endTime.Add(TimeSpan.FromMinutes(timeSpacing));
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex);
                    }

                }
                await unitOfWork.TripRepository.AddRangeAsync(trips);
                await unitOfWork.SaveChangesAsync();
                return await mediator.Send(new GetTripsByTimeTableIdQuery { TimeTableId = request.Id },
                    cancellationToken: cancellationToken);
            }
            else throw new Exception($"Timetable and Route is not valid");
        }
    }
}