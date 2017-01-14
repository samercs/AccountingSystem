using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using AccountingSystem.Core.Configration;
using AccountingSystem.Data;
using AccountingSystem.Entity;
using AccountingSystem.Service;
using AccountingSystem.Service.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AccountingSystem.Core.Service
{
    public class AuthService: IAuthService
    {
        private readonly HttpContextBase _httpContext;
        private readonly UserService _userService;
        private readonly SignInManager _signInManager;
        private readonly ICookieService _cookieService;

        public AuthService(HttpContextBase httpContext, ICookieService cookieService, IDataContextFactory dataContextFactory)
        {
            _httpContext = httpContext;
            _userService = new UserService(dataContextFactory);
            _signInManager = httpContext.GetOwinContext().Get<SignInManager>();
            _cookieService = cookieService;
        }

        public bool IsAuthenticated()
        {
            return _httpContext.Request.IsAuthenticated;
        }

        public bool IsLocal()
        {
            return _httpContext.Request.IsLocal;
        }

        public string CurrentUserId()
        {
            if (!IsAuthenticated())
            {
                return null;
            }

            return _httpContext.User.Identity.GetUserId();
        }

        public string AnonymousId()
        {
            return _httpContext.Request.AnonymousID;
        }

        public string UserHostAddress()
        {
            return _httpContext.Request.UserHostAddress;
        }

        public async Task<User> SignIn(string emailOrPhone, string password, bool rememberMe)
        {
            var user = await _userService.GetUserByEmail(emailOrPhone);

            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, false);
            if (result != SignInStatus.Success)
            {
                return null;
            }

            SetLoginCookies(user);

            return user;
        }

        public async Task<User> SignIn(User user)
        {
            await _signInManager.SignInAsync(user, false, false);
            SetLoginCookies(user);
            return user;
        }

        public void SignOut()
        {
            _httpContext.GetOwinContext().Authentication.SignOut();
        }

        public async Task<User> CurrentUser()
        {
            if (!IsAuthenticated())
            {
                return null;
            }

            return await _userService.GetUserById(CurrentUserId());
        }

        public async Task<IdentityResult> CreateUser(User user, string password, Func<User, Task> onSuccess = null, bool signIn = true)
        {
            var valid = await _userService.ValidateUserIdentity(user.Email, user.PhoneNumber);

            if (!valid)
            {
                return new IdentityResult("Email or phone number already exist in database");
            }

            var result = await _userService.CreateUser(user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            if (signIn)
            {
                await _signInManager.SignInAsync(user, false, false);
                SetLoginCookies(user);
            }

            if (onSuccess != null)
            {
                await onSuccess(user);
            }

            return result;
        }

        public async Task<IdentityResult> ChangePassword(IIdentity identity, string oldPassword, string newPassword)
        {
            return await _userService.ChangePassword(identity, oldPassword, newPassword);
        }

        private void SetLoginCookies(User user)
        {
            _cookieService.Add(CookieKeys.DisplayName, user.FirstName, DateTime.MaxValue);
            _cookieService.Add(CookieKeys.LastSignInEmail, user.Email, DateTime.MaxValue);
        }

        private void CheckUserIdentity(string email, string phoneNumber)
        {

        }
    }
}
