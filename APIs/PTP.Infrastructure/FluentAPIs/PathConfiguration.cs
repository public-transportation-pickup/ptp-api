// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using PTP.Domain.Entities;

// namespace PTP.Infrastructure.FluentAPIs;
// public class PathConfiguration : IEntityTypeConfiguration<Domain.Entities.Path>
// {
   
   
//     public void Configure(EntityTypeBuilder<Domain.Entities.Path> builder)
//     {
//         builder.HasOne(x => x.Route).WithMany().HasForeignKey(x => x.RouteId);
//         builder.HasKey(x => x.Id);
//         builder.HasOne(x => x.RouteVar).WithMany().HasForeignKey(x => x.RouteVarId);
//     }
// }