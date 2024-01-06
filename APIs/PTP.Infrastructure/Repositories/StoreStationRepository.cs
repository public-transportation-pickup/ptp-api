using PTP.Application.Repositories.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.Repositories;
public class StoreStationRepository : GenericRepository<StoreStation>, IStoreStationRepository
{
    public StoreStationRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
    }
}
