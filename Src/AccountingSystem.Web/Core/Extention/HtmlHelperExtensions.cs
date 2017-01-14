using System.Web.Mvc;
using AccountingSystem.Core.Base;

namespace AccountingSystem.Core.Extention
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString StatusMessage(this HtmlHelper htmlHelper, StatusMessage statusMessage)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("alert alert-success");//ToDo Fix Class
            div.InnerHtml += GetStatusMessageIcon(statusMessage.StatusMessageType);
            div.InnerHtml += "<span>" + statusMessage.Message + "</span>";
            div.InnerHtml += "<div class='clearfix'></div>";

            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString StatusMessage(this HtmlHelper htmlHelper, string message,
            StatusMessageType statusMessageType = StatusMessageType.Success)
        {
            var statusMessage = new StatusMessage(message, statusMessageType);
            return StatusMessage(htmlHelper, statusMessage);
        }

        private static string GetStatusMessageIcon(StatusMessageType statusMessageType)
        {
            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-3x pull-left");
            switch (statusMessageType)
            {
                case StatusMessageType.Success:
                    icon.AddCssClass("fa-check-square-o");
                    break;
                
                case StatusMessageType.Worng:
                    icon.AddCssClass("fa-warning");
                    break;
                case StatusMessageType.Dangers:
                    icon.AddCssClass("fa-exclamation-triangle");
                    break;
            }
            return icon.ToString();
        }
    }
}
