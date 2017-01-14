using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountingSystem.Controllers
{
    public class MaterialController : Controller
    {
        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Template2()
        {
            return View();
        }

        public ActionResult Template3(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            return View("Element");
        }
    }
}
