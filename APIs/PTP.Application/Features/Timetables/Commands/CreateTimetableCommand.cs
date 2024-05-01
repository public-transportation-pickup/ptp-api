using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PTP.Application.GlobalExceptionHandling.Exceptions;
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
            List<TimetableCreateModel> requestModel = new();
            foreach (var item in request.Models)
            {
                if (await unitOfWork.TimeTableRepository.FirstOrDefaultAsync(x => x.RouteId == item.RouteId && x.RouteVarId == item.RouteVarId) is null)
                {
                    requestModel.Add(item);
                }
            }
            var timetables = unitOfWork.Mapper.Map<IEnumerable<TimeTable>>(requestModel);
            await unitOfWork.TimeTableRepository.AddRangeAsync(timetables.ToList());
            await unitOfWork.SaveChangesAsync();
            var timetablesResult = new List<TimeTable>();
            foreach (var item in request.Models)
            {
                var timetable = await unitOfWork.TimeTableRepository.FirstOrDefaultAsync(x => x.RouteVarId == item.RouteVarId && x.RouteId == item.RouteId);
                if (timetable is not null)
                    timetablesResult.Add(timetable);
            }
            return timetablesResult;

        }
    }
}
