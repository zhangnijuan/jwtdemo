using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthJWTDemo.Models
{
    public class ActionAllowOriginAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            base.OnActionExecuting(filterContext);
        }
    }
}