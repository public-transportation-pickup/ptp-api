using MediatR;
using Microsoft.AspNetCore.Http.Metadata;
using PTP.Application.GlobalExceptionHandling.Exceptions;

namespace PTP.Application.Features.Timetables.Commands;
public class DeleteTimetableCommand : IRequest<bool>
{
    public Guid Id { get; set; } = default!;
    public class CommandHandler : IRequestHandler<DeleteTimetableCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteTimetableCommand request, CancellationToken cancellationToken)
        {
            var timetableTask = unitOfWork.TimeTableRepository.GetByIdAsync(request.Id);
            var tripTask = unitOfWork.TripRepository.WhereAsync(x => x.TimeTableId == request.Id);
            await Task.WhenAll(timetableTask, tripTask);
            unitOfWork.TimeTableRepository.SoftRemove(timetableTask.Result ?? throw new Exception($"no_data_found at {nameof(DeleteTimetableCommand)}"));
            unitOfWork.TripRepository.SoftRemoveRange(tripTask.Result);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}