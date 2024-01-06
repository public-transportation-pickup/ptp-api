using PTP.Application.Repositories.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
    }
}