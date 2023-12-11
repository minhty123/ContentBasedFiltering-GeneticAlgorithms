using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeacherManager.Models;

namespace TeacherManager.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MAKEUP_LESSONController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        // GET: Admin/MAKEUP_LESSON
        public ActionResult Index()
        {
            var mAKEUP_LESSON = db.MAKEUP_LESSON.Include(m => m.CLASSROOM).Include(m => m.ROOM).Include(m => m.SUBJECT).Where(m => m.SITUATION=="Đã duyệt").OrderByDescending(m => m.DATE);
            var ls = db.MAKEUP_LESSON.Include(m => m.CLASSROOM).Include(m => m.ROOM).Include(m => m.SUBJECT).Where(m => m.SITUATION != "Đã duyệt").OrderByDescending(m => m.DATE);
            ViewBag.ls = ls;
            return View(mAKEUP_LESSON.ToList());
        }

        // GET: Admin/MAKEUP_LESSON/Details/5
        public ActionResult Details(int? id)
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
            return View(mAKEUP_LESSON);
        }

        public ActionResult Confirm(int? id)
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
            return View(mAKEUP_LESSON);
        }

        // GET: Admin/MAKEUP_LESSON/Delete/5
        public ActionResult Delete(int? id)
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
            return View(mAKEUP_LESSON);
        }

        // POST: Admin/MAKEUP_LESSON/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MAKEUP_LESSON mAKEUP_LESSON = db.MAKEUP_LESSON.Find(id);
            db.MAKEUP_LESSON.Remove(mAKEUP_LESSON);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
