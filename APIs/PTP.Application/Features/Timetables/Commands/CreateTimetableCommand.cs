using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using PTP.Application.ViewModels.Timetables;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Timetables.Commands;
public class CreateTimetableCommand : IRequest<List<TimeTable>?>
{
    public List<TimetableCreateModel> Models { get; set; } = default!;
    public class CommandValidation : AbstractValidator<CreateTimetableCommand>
    {
        public CommandValidation()
        {
            RuleForEach(x => x.Models).SetValidator(new TimeTableCreateModelValidator());
        }
    }
    public class TimeTableCreateModelValidator : AbstractValidator<TimetableCreateModel>
    {
        public TimeTableCreateModelValidator()
        {
            RuleFor(x => x.ApplyDates).NotNull().NotEmpty();
        }
    }
    public class CommandHandler : IRequestHandler<CreateTimetableCommand, List<TimeTable>?>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<TimeTable>?> Handle(CreateTimetableCommand request, CancellationToken cancellationToken)
        {
            var timetables = unitOfWork.Mapper.Map<IEnumerable<TimeTable>>(request.Models);
            await unitOfWork.TimeTableRepository.AddRangeAsync(timetables.ToList());
            await unitOfWork.SaveChangesAsync();
            return timetables.ToList();

        }
    }
}
