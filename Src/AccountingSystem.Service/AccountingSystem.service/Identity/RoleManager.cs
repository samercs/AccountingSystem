using System.Data.Entity;
using AccountingSystem.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AccountingSystem.Service.Identity
{
    public class RoleManager: RoleManager<IdentityRole>
    {
        public RoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
        }

        public RoleManager(IDataContextFactory dataContextFactory)
            : base(new RoleStore<IdentityRole>((DbContext)dataContextFactory.GetContext()))
        {
        }
    }
}
