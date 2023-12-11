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
    public class APPLICATION_LEAVEController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        // GET: Admin/APPLICATION_LEAVE
        public ActionResult Index()
        {
            var aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Include(a => a.TEACHER).Where(m => m.STATUS=="Đã duyệt").OrderByDescending(m => m.DATESTART);
            var ls = db.APPLICATION_LEAVE.Include(a => a.TEACHER).Where(m => m.STATUS != "Đã duyệt").ToList();
            ViewBag.ls = ls;
            return View(aPPLICATION_LEAVE.ToList());
        }

        // GET: Admin/APPLICATION_LEAVE/Details/5
        public ActionResult Details(int? id)
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
            return View(aPPLICATION_LEAVE);
        }

        public ActionResult Confirm(int? id)
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
            return View(aPPLICATION_LEAVE);
        }

        // GET: Admin/APPLICATION_LEAVE/Delete/5
        public ActionResult Delete(int? id)
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
            return View(aPPLICATION_LEAVE);
        }

        // POST: Admin/APPLICATION_LEAVE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            APPLICATION_LEAVE aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Find(id);
            db.APPLICATION_LEAVE.Remove(aPPLICATION_LEAVE);
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
