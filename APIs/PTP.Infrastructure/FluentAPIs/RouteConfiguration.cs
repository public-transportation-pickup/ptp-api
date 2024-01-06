using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.RouteStations).WithOne(x => x.Route).HasForeignKey(x => x.RouteId);
        builder.HasMany(x => x.RouteVars).WithOne(x => x.Route).HasForeignKey(x => x.RouteId);
        builder.Property(x => x.RouteId).ValueGeneratedNever();
        
    }
}