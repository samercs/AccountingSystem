using AccountingSystem.Entity;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AccountingSystem.Data
{
    public interface IDataContext : IDisposable
    {
        IDbSet<Account> Accounts { get; set; }
        int SaveChange();
        Task<int> SaveChangeAsyn();
        void SetModified(object entity);
    }
}
