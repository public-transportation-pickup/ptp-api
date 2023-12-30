using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Vehicle).WithMany(x => x.Trips).HasForeignKey(x => x.VehicleId);
        builder.HasOne(x => x.Route).WithMany(x => x.Trips).HasForeignKey(x => x.RouteId);
    }
}