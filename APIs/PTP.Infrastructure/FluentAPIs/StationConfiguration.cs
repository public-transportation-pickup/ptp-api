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
        builder.HasMany(x => x.StoreStations).WithOne(x => x.Station).HasForeignKey(x => x.StationId);
    }
}