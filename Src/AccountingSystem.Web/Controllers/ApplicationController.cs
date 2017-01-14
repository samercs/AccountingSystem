using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccountingSystem.Core.Base;
using AccountingSystem.Core.Service;
using AccountingSystem.Data;

namespace AccountingSystem.Controllers
{
    public class ApplicationController : Controller
    {
        protected readonly IDataContextFactory DataContextFactory;
        protected readonly IAuthService AuthService;
        protected readonly ICookieService CookieService;

        public ApplicationController(IAppService appService)
        {
            DataContextFactory = appService.DataContextFactory;
            AuthService = appService.AuthService;
            CookieService = appService.CookieService;
        }
       
        

        public void SetStatusMessage(string message, StatusMessageType statusMessageType = StatusMessageType.Success)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            TempData["StatusMessage"] = new StatusMessage(message, statusMessageType);
        }

        public StatusMessage GetStatusMessage()
        {
            return TempData["StatusMessage"] as StatusMessage;
        }
    }
}
