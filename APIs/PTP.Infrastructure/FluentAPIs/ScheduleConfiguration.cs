using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Trip).WithMany(x => x.Schedules).HasForeignKey(x => x.TripId);
        builder.HasOne(x => x.Station).WithMany(x => x.Schedules).HasForeignKey(x => x.StationId);
    
    }
}
