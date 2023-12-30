using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Infrastructure.FluentAPIs;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Users).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        builder.HasData(
            new Role { Name = nameof(RoleEnum.StoreManager) },
            new Role { Name = nameof(RoleEnum.Customer) },
            new Role { Name = nameof(RoleEnum.Admin) },
            new Role { Name = nameof(RoleEnum.TransportationEmployee) });

    }
}