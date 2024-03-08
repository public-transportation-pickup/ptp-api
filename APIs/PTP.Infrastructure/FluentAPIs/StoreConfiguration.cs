using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Stations).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.Products).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
        builder.HasMany(x => x.Menus).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
        // builder.HasOne(x => x.Wallet).WithOne(x => x.Store).HasForeignKey<Store>(x => x.WalletId);
        builder.HasOne(x => x.User).WithOne(x => x.Store).HasForeignKey<Store>(x => x.UserId);
        builder.HasMany(x => x.Orders).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
    }
}