using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class StoreStationConfiguration : IEntityTypeConfiguration<StoreStation>
{
    public void Configure(EntityTypeBuilder<StoreStation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Station).WithMany(x => x.StoreStations).HasForeignKey(x => x.StationId);
        builder.HasOne(x => x.Store).WithMany(x => x.StoreStations).HasForeignKey(x => x.StoreId);
    }
}
