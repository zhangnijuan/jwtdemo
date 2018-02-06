using AuthJWTDemo.Models;
using System.Web;
using System.Web.Mvc;

namespace AuthJWTDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionAllowOriginAttribute());
        }
    }
}
