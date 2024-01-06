using PTP.Application.Repositories.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.Repositories;
public class TimeTableRepository : GenericRepository<TimeTable>, ITimeTableRepository
{
    public TimeTableRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
    }
}