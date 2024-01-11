using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class RouteVarConfiguration : IEntityTypeConfiguration<RouteVar>
{
    public void Configure(EntityTypeBuilder<RouteVar> builder)
    {
        builder.HasKey(x => x.Id);
        //builder.HasMany(x => x.Paths).WithOne(x => x.RouteVar).HasForeignKey(x => x.RouteVarId);
        builder.HasMany(x => x.RouteStations).WithOne(x => x.RouteVar).HasForeignKey(x => x.RouteVarId)
        .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Route).WithMany(x => x.RouteVars).HasForeignKey(x => x.RouteId);
        builder.HasMany(x => x.Timetables).WithOne(x => x.RouteVar).HasForeignKey(x => x.RouteVarId);
    }
}