using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Store).WithMany(x => x.Sessions).HasForeignKey(x => x.StoreId);
        builder.HasOne(x => x.TimeFrame).WithMany(x => x.Sessions).HasForeignKey(x => x.TimeFrameId);
    }
}