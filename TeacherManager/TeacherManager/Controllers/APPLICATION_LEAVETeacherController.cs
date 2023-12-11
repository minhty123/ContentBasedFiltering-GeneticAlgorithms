using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeacherManager.Models;

namespace TeacherManager.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class APPLICATION_LEAVETeacherController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        // GET: APPLICATION_LEAVETeacher
        public ActionResult Index()
        {
            var aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Include(a => a.TEACHER);
            return View(aPPLICATION_LEAVE.ToList());
        }

        // GET: APPLICATION_LEAVETeacher/Details/5
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

       
        // GET: APPLICATION_LEAVETeacher/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ID_TEACHER = new SelectList(db.TEACHERs, "ID", "NAME", aPPLICATION_LEAVE.ID_TEACHER);
            return View(aPPLICATION_LEAVE);
        }

        // POST: APPLICATION_LEAVETeacher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_TEACHER,DATESTART,REASON,STATUS,DATEEND,TYPELEAVE")] APPLICATION_LEAVE aPPLICATION_LEAVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aPPLICATION_LEAVE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_TEACHER = new SelectList(db.TEACHERs, "ID", "NAME", aPPLICATION_LEAVE.ID_TEACHER);
            return View(aPPLICATION_LEAVE);
        }

        // GET: APPLICATION_LEAVETeacher/Delete/5
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

        // POST: APPLICATION_LEAVETeacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            APPLICATION_LEAVE aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Find(id);
            db.APPLICATION_LEAVE.Remove(aPPLICATION_LEAVE);
            db.SaveChanges();
            return RedirectToAction("GetApplicaMakeupLessonForTeacher","Home", new { areas = "" });
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
