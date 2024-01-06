using PTP.Application.Repositories.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.Repositories;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
    }
}