using System.Web;
using System.Web.Mvc;

namespace BE2__ADMINLAYOUT
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
