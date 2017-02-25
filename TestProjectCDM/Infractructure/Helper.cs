using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TestProjectCDM.Infractructure
{
    public static class Helper
    {
        /// <summary>
        /// This method is decoration around .ActionLink() and helps input HTML into linkText
        /// </summary>
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName,
            object routeValues,AjaxOptions ajaxOptions)
        {
            var repId = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repId, actionName,routeValues, ajaxOptions);
            return MvcHtmlString.Create(lnk.ToString().Replace(repId, linkText));
        }
    }
}