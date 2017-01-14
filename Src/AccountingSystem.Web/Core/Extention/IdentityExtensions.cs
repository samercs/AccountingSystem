using System.Security.Principal;
using System.Web;
using AccountingSystem.Core.Configration;
using AccountingSystem.Core.Service;
using Microsoft.AspNet.Identity;

namespace AccountingSystem.Core.Extention
{
    public static class IdentityExtensions
    {
        public static string GetDisplayName(this IIdentity identity)
        {
            var cookieService = new CookieService(new HttpContextWrapper(HttpContext.Current));
            var displayName = cookieService.Get(CookieKeys.DisplayName);
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = identity.GetUserName();
            }

            return displayName;
        }
    }
}
