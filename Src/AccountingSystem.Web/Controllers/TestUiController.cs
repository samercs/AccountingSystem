
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccountingSystem.Core.Service;
using AccountingSystem.Models.TestUi;
using AccountingSystem.Service;


namespace AccountingSystem.Controllers
{
    public class TestUiController : ApplicationController
    {
        
        public TestUiController(IAppService appService) : base(appService)
        {
            
        }

        // GET: TestUi
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            return Content(model.Name.ToString());
        }

        


    }
}
