using System.Web.Mvc;

namespace TeacherManager.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "ApplicationForLeave_Manager",
               "Admin/don-xin-nghi-phep",
               new { controller = "APPLICATION_LEAVE", action = "Index"},
                namespaces: new[] { "TeacherManager.Areas.Admin.Controllers" }
            );
            context.MapRoute(
               "MakeupLesson_Manager",
               "Admin/don-dang-ky",
               new { controller = "MAKEUP_LESSON", action = "Index" },
                namespaces: new[] { "TeacherManager.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}