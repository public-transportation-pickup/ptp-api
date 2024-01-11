using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.RouteStations).WithOne(x => x.Station).HasForeignKey(x => x.StationId);
        builder.HasOne(x => x.Store).WithMany(x => x.Stations).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.Orders).WithOne(x => x.Station).HasForeignKey(x => x.StationId);
        builder.HasMany(x => x.Schedules).WithOne(x => x.Station).HasForeignKey(x => x.StationId);
    }
}