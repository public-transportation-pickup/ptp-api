using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class TimeTableConfiguration : IEntityTypeConfiguration<TimeTable>
{
    public void Configure(EntityTypeBuilder<TimeTable> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Trips).WithOne(x => x.TimeTable).HasForeignKey(x => x.TimeTableId);
        builder.HasOne(x => x.RouteVar).WithMany(x => x.Timetables).HasForeignKey(x => x.RouteVarId);

    }
}