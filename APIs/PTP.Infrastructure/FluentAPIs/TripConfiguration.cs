using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Schedules).WithOne(x => x.Trip).HasForeignKey(x => x.TripId);
        builder.HasOne(x => x.Route).WithMany(x => x.Trips).HasForeignKey(x => x.RouteId);
    }
}