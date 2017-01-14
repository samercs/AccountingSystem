using AccountingSystem.Data;
using AccountingSystem.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AccountingSystem.Service.Identity
{
    public class SignInManager : SignInManager<User, string>
    {
        public SignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
        {
            return new SignInManager(new UserManager(new DataContextFactory()), context.Authentication);
        }
    }
}
