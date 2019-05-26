using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedPortal.Data.Persistence {
	public class DataContext : DbContext, IDataContext {
		public DataContext(DbContextOptions<DataContext> options)
			: base(options) {
		}

		public DbSet<HCity> Cities { get; set; }
		public DbSet<HClinic> Clinics { get; set; }
		public DbSet<HDistrict> Districs { get; set; }
		public DbSet<HDoctor> Doctors { get; set; }
		public DbSet<HSpeciality> Specialities { get; set; }
		public DbSet<HStation> Stations { get; set; }
		public DbSet<HStreet> Streets { get; set; }
		public DbSet<HTelemed> Telemeds { get; set; }
		public DbSet<Log> Logs { get; set; }

		public DbSet<HClinicDoctors> ClinicDoctors { get; set; }
		public DbSet<HClinicStations> ClinicStations { get; set; }
		public DbSet<HDistrictStations> DistrictStations { get; set; }
		public DbSet<HDoctorSpecialities> DoctorSpecialities { get; set; }

		public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel) {
			return Database.BeginTransaction(isolationLevel);
		}

		public async Task<int> SaveChangesAsync() {
			return await SaveChangesAsync(true);
		}

		public async Task<int> ExecuteSqlCommand(string command) {
			return await Database.ExecuteSqlCommandAsync(command);
		}

		public string GetTableName<T>(DbSet<T> dbSet) where T : class, IEntity {
			var model = this.Model;
			var entityTypes = model.GetEntityTypes();
			var entityType = entityTypes.First(t => t.ClrType == typeof(T));
			var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
			var tableName = tableNameAnnotation.Value.ToString();
			return tableName;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var relationship in modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetForeignKeys())) {
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}

			modelBuilder.Entity<HCity>().HasIndex(c => c.OriginId).IsUnique();
			modelBuilder.Entity<HStreet>().HasIndex(c => c.OriginId).IsUnique();
			modelBuilder.Entity<HDistrict>().HasIndex(c => c.OriginId).IsUnique();
			modelBuilder.Entity<HStation>().HasIndex(c => c.OriginId).IsUnique();

			modelBuilder.Entity<HClinic>().HasIndex(c => c.OriginId).IsUnique();
			modelBuilder.Entity<HDoctor>().HasIndex(c => c.OriginId).IsUnique();
			modelBuilder.Entity<HSpeciality>().HasIndex(c => c.OriginId).IsUnique();
			modelBuilder.Entity<HTelemed>().HasIndex(c => c.OriginId).IsUnique();

			base.OnModelCreating(modelBuilder);
		}
	}
}