using AccountingSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Data
{
    public interface IDataContext : IDisposable
    {
        DbSet<Account> Accounts { get; set; }
        int SaveChange();
        Task<int> SaveChangeAsyn();
        void SetModified(object entity);
    }
}
