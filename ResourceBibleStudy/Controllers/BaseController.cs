using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ResourceBibleStudy.Helpers;
using ResourceBibleStudy.Models;

namespace ResourceBibleStudy.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {

        /// <summary>
        /// View To string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        public string GetIpAddress()
        {
            var szRemoteAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
            var szXForwardedFor = System.Web.HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
            string szIp;

            if (string.IsNullOrEmpty(szXForwardedFor))
            {
                szIp = szRemoteAddr;
            }
            else
            {
                szIp = szXForwardedFor;
                if (szIp.IndexOf(",", StringComparison.Ordinal) <= 0) return szIp;
                var arIPs = szIp.Split(',');

                foreach (var item in arIPs.Where(item => !string.IsNullOrEmpty(item)))
                {
                    return item;
                }
            }
            return szIp;
        }
        protected UserDetail GetCurrentUser()
        {
            if (UserSession.Current.LoggedInUser != null) return UserSession.Current.LoggedInUser;

            var user = new UserDetail
            {
                UserName = string.Format("{0}-{1}", System.Web.HttpContext.Current.Request.Browser.Browser, GetIpAddress()),

            };
            return (UserSession.Current.LoggedInUser = user);
        }

    }

}