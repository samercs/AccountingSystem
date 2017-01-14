using AccountingSystem.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AccountingSystem.Data
{
    public class DataContext : IdentityDbContext<User>, IDataContext
    {
        public IDbSet<Account> Accounts { get; set; }

        public DataContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(prop => prop.HasPrecision(18, 3));
            base.OnModelCreating(modelBuilder);
        }
        public int SaveChange()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
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
            catch (DbEntityValidationException ex)
            {
                TraceValidationErrors(ex);
                throw;
            }
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        private static void TraceValidationErrors(DbEntityValidationException ex)
        {
            foreach (var validationErrors in ex.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceError("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }
        }

        public static DataContext Create()
        {
            return new DataContext();
        }
    }
}
