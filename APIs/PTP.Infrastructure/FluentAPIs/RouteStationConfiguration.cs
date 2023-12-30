using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class RouteStationConfiguration : IEntityTypeConfiguration<RouteStation>
{
    public void Configure(EntityTypeBuilder<RouteStation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Route).WithMany(x => x.RouteStations).HasForeignKey(x => x.RouteId);
        builder.HasOne(x => x.Station).WithMany(x => x.RouteStations).HasForeignKey(x => x.StationId);
       
    }
}
