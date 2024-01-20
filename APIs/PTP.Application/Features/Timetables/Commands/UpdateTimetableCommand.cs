using MediatR;
using Microsoft.VisualBasic;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.ViewModels.Timetables;

namespace PTP.Application.Features.Timetables.Commands;
public class UpdateTimetableCommand : IRequest
{
    public Guid Id { get; set; } = default!;
    public TimetableUpdateModel Model { get; set; } = default!;
    public class CommandHandler : IRequestHandler<UpdateTimetableCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateTimetableCommand request, CancellationToken cancellationToken)
        {
            var timetable = await unitOfWork.TimeTableRepository.GetByIdAsync(request.Id) ??
                                    throw new NotFoundException($"no_data_found at {nameof(UpdateTimetableCommand)}");

            unitOfWork.Mapper.Map(request.Model, timetable);
            unitOfWork.TimeTableRepository.Update(timetable);
            await unitOfWork.SaveChangesAsync();
        }
    }
}