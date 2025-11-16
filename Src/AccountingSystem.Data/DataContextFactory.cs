using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccountingSystem.Data
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly IConfiguration _configuration;

        public DataContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDataContext GetContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            return new DataContext(optionsBuilder.Options);
        }
    }
}
