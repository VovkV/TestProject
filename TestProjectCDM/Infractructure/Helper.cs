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
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName,
            AjaxOptions ajaxOptions)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, ajaxOptions);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }
    }
}