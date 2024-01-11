using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PTP.Domain.Entities;

namespace PTP.Infrastructure
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		#region DbSets
		public DbSet<Role> Role { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<WalletLog> WalletLog { get; set; }
		public DbSet<Wallet> Wallet { get; set; }
		public DbSet<Transaction> Transaction { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<Menu> Menu { get; set; }
		public DbSet<Order> Order { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Payment> Payment { get; set; }
		public DbSet<Product> Product { get; set; }
		public DbSet<ProductImage> ProductImage { get; set; }
		public DbSet<ProductInMenu> ProductInMenu { get; set; }
		public DbSet<Route> Route { get; set; }
		public DbSet<RouteStation> RouteStation { get; set; }
		public DbSet<Station> Station { get; set; }
		public DbSet<Store> Store { get; set; }

		public DbSet<Trip> Trip { get; set; }
		public DbSet<Schedule> Schedules { get; set; } = default!;
		public DbSet<TimeTable> TimeTables { get; set; } = default!;
		public DbSet<RouteVar> RouteVars { get; set; } = default!;
		//public DbSet<Domain.Entities.Path> Path { get; set; } = default!;

		#endregion
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
		}
	}
}
