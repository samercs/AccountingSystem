using System;
using System.Security.Principal;
using System.Threading.Tasks;
using AccountingSystem.Entity;
using Microsoft.AspNet.Identity;

namespace AccountingSystem.Core.Service
{
    public interface IAuthService
    {
        bool IsAuthenticated();
        bool IsLocal();
        string CurrentUserId();
        string AnonymousId();
        string UserHostAddress();

        Task<User> SignIn(string email, string password, bool rememberMe);
        Task<User> SignIn(User user);
        void SignOut();

        Task<User> CurrentUser();

        Task<IdentityResult> CreateUser(User user, string password, Func<User, Task> onSuccess = null, bool signIn = true);
        Task<IdentityResult> ChangePassword(IIdentity identity, string oldPassword, string newPassword);

    }
}
