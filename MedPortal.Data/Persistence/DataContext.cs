using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedPortal.Data.Persistence
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options) {   
        }

        public DbSet<HBranch> Branches { get; set; }
        public DbSet<HCity> Cities { get; set; }
        public DbSet<HClinic> Clinics { get; set; }
        public DbSet<HDistrict> Districs { get; set; }
        public DbSet<HDoctor> Doctors { get; set; }
        public DbSet<HSpeciality> Specialities { get; set; }
        public DbSet<HStation> Stations { get; set; }
        public DbSet<HStreet> Streets { get; set; }
        public DbSet<HTelemed> Telemeds { get; set; }
        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Database.BeginTransaction(isolationLevel);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
                        
            base.OnModelCreating(modelBuilder);
        }
        
        
    }
}