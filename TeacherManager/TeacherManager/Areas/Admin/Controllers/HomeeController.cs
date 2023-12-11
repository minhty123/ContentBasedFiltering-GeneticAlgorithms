using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeacherManager.Models;

namespace TeacherManager.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeeController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();
        // GET: Admin/Home

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CountItems() { 
            int count = db.APPLICATION_LEAVE.Where(m => m.STATUS == "Đang chờ duyệt").Count() + db.MAKEUP_LESSON.Where(m => m.SITUATION == "Đang chờ duyệt").Count();
            return PartialView("CountItems",count); 
        }

        public ActionResult RenderListMakeupLesson()
        {
            var result = db.MAKEUP_LESSON.Where(m => m.SITUATION == "Đang chờ duyệt").ToList();
            return PartialView("MakeupLessonNumber", result);
        }

        public ActionResult RenderListApplicationForLeave()
        {
            var result = db.APPLICATION_LEAVE.Where(m => m.STATUS == "Đang chờ duyệt").ToList();
            return PartialView("ApplicationForLeaveNumber", result);
        }

        public ActionResult ChangeStatusApplicationLeave(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPLICATION_LEAVE aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Find(id);
            if (aPPLICATION_LEAVE == null)
            {
                return HttpNotFound();
            }
            aPPLICATION_LEAVE.STATUS = "Đã duyệt";
            db.Entry(aPPLICATION_LEAVE).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "APPLICATION_LEAVE", new { areas = "Admin" });
        }

        public ActionResult ChangeStatusMakeupLesson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAKEUP_LESSON mAKEUP_LESSON = db.MAKEUP_LESSON.Find(id);
            if (mAKEUP_LESSON == null)
            {
                return HttpNotFound();
            }
            mAKEUP_LESSON.SITUATION = "Đã duyệt";
            db.Entry(mAKEUP_LESSON).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "MAKEUP_LESSON", new { areas = "Admin" });
        }

    }
}