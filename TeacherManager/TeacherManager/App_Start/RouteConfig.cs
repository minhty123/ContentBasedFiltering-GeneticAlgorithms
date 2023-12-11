using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeacherManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes.MapRoute(
            //    name: "AdminLogout1",
            //    url: "Admin/Default/Logout",
            //    defaults: new { controller = "Default", action = "Logout" },
            //    namespaces: new[] { "TeacherManager.Areas.Admin.Controllers" } // nếu cần
            //);GetApplicaMakeupLessonForTeacher
            routes.MapRoute(
               name: "GetApplicaMakeupLessonForTeacher",
               url: "quan-ly-don-xin-phep-don-day-bu",
               defaults: new { controller = "Home", action = "GetApplicaMakeupLessonForTeacher"}
           );
           // routes.MapRoute(
           //    name: "DetailsNotification",
           //    url: "thong-bao/{name}",
           //    defaults: new { controller = "NOTIFICATIONs", action = "Details", name = (string)null }
           //);
           // routes.MapRoute(
           //     name: "DetailsNew",
           //     url: "tin-tuc/{name}",
           //     defaults: new { controller = "NEWS", action = "Details",name=(string)null }
           // );
            routes.MapRoute(
                name: "RegisterApplicationForLeave",
                url: "dang-ky-nghi-phep",
                defaults: new { controller = "TeachingSchedule", action = "RegisterApplicationForLeave" }
            );
            routes.MapRoute(
                name: "AccountManage",
                url: "quan-ly-tai-khoan",
                defaults: new { controller = "Manage", action = "ReviewEditInforTeacher" }
            );
            routes.MapRoute(
                name: "TimetableView",
                url: "lich-giang-day",
                defaults: new { controller = "TeachingSchedule", action = "Timetable" }
            );
            routes.MapRoute(
                name: "RegisterMakeupLesson",
                url: "dang-ky-day-bu",
                defaults: new { controller = "TeachingSchedule", action = "Register" }
            );


            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
