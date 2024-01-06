using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class WalletLogConfiguration : IEntityTypeConfiguration<WalletLog>
{
    public void Configure(EntityTypeBuilder<WalletLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Wallet).WithMany(x => x.WalletLogs).HasForeignKey(x => x.WalletId);
    }
}