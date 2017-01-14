using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccountingSystem.Core.Service;

namespace AccountingSystem.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IAuthService _authService;
        public HomeController(IAppService appService) : base(appService)
        {
            _authService = AuthService;
        }
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Login", "Account");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Cross Over C# Software Engineer project";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "For any inquery please contact me on: ";

            return View();
        }


    }
}
