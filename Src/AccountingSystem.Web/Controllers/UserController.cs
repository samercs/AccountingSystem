using AccountingSystem.Core.Service;
using AccountingSystem.Models;
using AccountingSystem.Service;
using AccountingSystem.Service.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AccountingSystem.Controllers
{
    [Authorize]
    public class UserController : ApplicationController
    {
        private readonly IAuthService _authService;
        private readonly UserService _userService;

        private readonly UserManager _userManager;
        private readonly ApiService _apiService;
        public UserController(IAppService appService) : base(appService)
        {
            _authService = appService.AuthService;
            _userService = new UserService(DataContextFactory);
            _userManager = new UserManager(DataContextFactory);
            _apiService = new ApiService();
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult SetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.CurrentUser();
                var result = await _userManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    SetStatusMessage("password has been update successfully.");
                    return RedirectToAction("Index", "User");
                }
                SetStatusMessage("Error: can't change your password");
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
