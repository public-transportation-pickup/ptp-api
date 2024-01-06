using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class RouteVarConfiguration : IEntityTypeConfiguration<RouteVar>
{
    public void Configure(EntityTypeBuilder<RouteVar> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Route).WithMany(x => x.RouteVars).HasForeignKey(x => x.RouteId);
        builder.HasOne(x => x.TimeTable).WithOne(x => x.RouteVar).HasForeignKey<TimeTable>(x => x.RouteVarId);
    }
}