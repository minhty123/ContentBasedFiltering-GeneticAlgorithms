using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherManager.Models;
using TeacherManager.Models.Class;

namespace TeacherManager.Controllers
{
    public class HomeController : Controller
    {
        TeacherWorkEntities db = new TeacherWorkEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetApplicaMakeupLessonForTeacher()
        {
            return View();
        }

    }
}