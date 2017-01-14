using System;
using System.Data.Entity;
using AccountingSystem.Data;
using AccountingSystem.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AccountingSystem.Service.Identity
{
    public class UserManager: UserManager<User>
    {
        public UserManager(IUserStore<User> store) : base(store)
        {
            InitUserManager();
        }

        public UserManager(IDataContextFactory dataContextFactory)
            : base(new UserStore<User>((DbContext)dataContextFactory.GetContext()))
        {
            InitUserManager();
        }

        private void InitUserManager()
        {
            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            MaxFailedAccessAttemptsBeforeLockout = 10;


        }
    }
}
