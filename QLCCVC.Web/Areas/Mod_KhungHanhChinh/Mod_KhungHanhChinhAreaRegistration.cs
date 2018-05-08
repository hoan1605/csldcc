using System.Web.Mvc;

namespace QLCCVC.Web.Areas.Mod_khungHanhChinh
{
    public class Mod_khungHanhChinhAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mod_khungHanhChinh";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mod_khungHanhChinh_default",
                "Mod_khungHanhChinh/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}