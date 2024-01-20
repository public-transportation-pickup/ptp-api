using MediatR;
using PTP.Application.ViewModels.Timetables;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Timetables.Commands;
public class CreateTimetableCommand : IRequest<bool>
{
    public List<TimetableCreateModel> Models { get; set; } = default!;
    public class CommandHandler : IRequestHandler<CreateTimetableCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork) 
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CreateTimetableCommand request, CancellationToken cancellationToken)
        {
            var timetables = unitOfWork.Mapper.Map<IEnumerable<TimeTable>>(request.Models);
            await unitOfWork.TimeTableRepository.AddRangeAsync(timetables.ToList());
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
