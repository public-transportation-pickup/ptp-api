using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.StoreStations).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.Products).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.Menus).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.Sessions).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
    }
}
