using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.FluentAPIs;
public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
{
    public void Configure(EntityTypeBuilder<MenuCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Menu).WithMany(x => x.MenuCategories).HasForeignKey(x => x.MenuId);
        builder.HasOne(x => x.Category).WithMany(x => x.MenuCategories).HasForeignKey(x => x.CategoryId);
    }
}