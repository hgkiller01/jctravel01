using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01
{
    public class AuthorizeOurCompnyAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["ComnpanyNo"].ToString() != "00001")
            {
                filterContext.HttpContext.Response.Redirect("~/Home/Index");
            }
        }
    }
 
}