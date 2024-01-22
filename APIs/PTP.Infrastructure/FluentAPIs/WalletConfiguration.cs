using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.WalletLogs).WithOne(x => x.Wallet).HasForeignKey(x => x.WalletId);
        builder.HasOne(x => x.User).WithOne(x => x.Wallet).HasForeignKey<Wallet>(x => x.UserId);
        builder.HasOne(x => x.Store).WithOne(x => x.Wallet).HasForeignKey<Wallet>(x => x.StoreId);
        builder.HasMany(x => x.Transactions).WithOne(x => x.Wallet).HasForeignKey(x => x.WalletId);
    }
}
