using AccountingSystem.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSystem.Data
{
    public class DataContext : IdentityDbContext<User>, IDataContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision for all decimal properties
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(3);
            }

            base.OnModelCreating(modelBuilder);
        }

        public int SaveChange()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                TraceValidationErrors(ex);
                throw;
            }
        }

        public async Task<int> SaveChangeAsyn()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TraceValidationErrors(ex);
                throw;
            }
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        private static void TraceValidationErrors(Exception ex)
        {
            Trace.TraceError("Error: {0}", ex.Message);
            if (ex.InnerException != null)
            {
                Trace.TraceError("Inner Error: {0}", ex.InnerException.Message);
            }
        }

        public static DataContext Create(DbContextOptions<DataContext> options)
        {
            return new DataContext(options);
        }
    }
}
