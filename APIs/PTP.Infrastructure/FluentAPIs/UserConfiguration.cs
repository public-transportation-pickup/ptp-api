using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Wallet).WithOne(x => x.User).HasForeignKey<Wallet>(x => x.UserId);
        builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);
        builder.HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Store).WithOne(x => x.User).HasForeignKey<User>(x => x.StoreId);
    }
}