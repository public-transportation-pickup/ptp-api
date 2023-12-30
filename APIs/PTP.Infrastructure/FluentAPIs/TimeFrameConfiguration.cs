using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class TimeFrameConfiguration : IEntityTypeConfiguration<TimeFrame>
{
    public void Configure(EntityTypeBuilder<TimeFrame> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Sessions).WithOne(x => x.TimeFrame).HasForeignKey(x => x.TimeFrameId);
    }
}
