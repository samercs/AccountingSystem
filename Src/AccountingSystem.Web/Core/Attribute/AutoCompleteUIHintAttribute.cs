using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AccountingSystem.Core.Attribute
{
    public class AutoCompleteUiHintAttribute: UIHintAttribute
    {
        private readonly UrlHelper _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public string DataSourceUri { get; set; }
        public AutoCompleteUiHintAttribute(string url) : base("AutoComplete")
        {
            DataSourceUri = url;
        }

        public AutoCompleteUiHintAttribute(string action, string controller) : base("AutoComplete")
        {
            DataSourceUri = _urlHelper.RouteUrl(new { action = action, controller = controller });
        }

        public AutoCompleteUiHintAttribute(string action, string controller, string area) :base("AutoComplete")
        {
            DataSourceUri = _urlHelper.RouteUrl(new { action = action, controller = controller, area = area });
        }


    }
}
