using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jctravel01.Controllers;

namespace jctravel01
{
    public class AuthorizeCompnyAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["ComnpanyNo"] == null &&
                filterContext.HttpContext.Session["UserID"] == null && filterContext.HttpContext.Session["UserName"] == null)
            {
                var now = filterContext.Controller;
                now.TempData["LogOut"] = "請重新登入";
                filterContext.RequestContext.HttpContext.Session["Relogin"] = "請重新登入";
                now.TempData["LoginUrl"] = now.ControllerContext.HttpContext.Request.Url.ToString();
                filterContext.HttpContext.Response.Redirect("~/MemberLogin");
            }           
        }
      
    }
}