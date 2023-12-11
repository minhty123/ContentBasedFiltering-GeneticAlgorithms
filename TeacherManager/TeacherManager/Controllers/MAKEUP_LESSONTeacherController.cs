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
    public class MAKEUP_LESSONTeacherController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        // GET: MAKEUP_LESSONTeacher
        public ActionResult Index()
        {
            var mAKEUP_LESSON = db.MAKEUP_LESSON.Include(m => m.CLASSROOM).Include(m => m.ROOM).Include(m => m.SUBJECT);
            return View(mAKEUP_LESSON.ToList());
        }

        // GET: MAKEUP_LESSONTeacher/Details/5
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

        // GET: MAKEUP_LESSONTeacher/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ID_CLASS = new SelectList(db.CLASSROOMs, "ID", "NAME", mAKEUP_LESSON.ID_CLASS);
            ViewBag.ID_ROOM = new SelectList(db.ROOMs, "ID", "NAME_ROM", mAKEUP_LESSON.ID_ROOM);
            ViewBag.ID_SUBJECT = new SelectList(db.SUBJECTs, "ID", "NAME", mAKEUP_LESSON.ID_SUBJECT);
            return View(mAKEUP_LESSON);
        }

        // POST: MAKEUP_LESSONTeacher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_CLASS,ID_SUBJECT,DATE,TIMESTART,TIMEEND,SITUATION,ID_ROOM")] MAKEUP_LESSON mAKEUP_LESSON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mAKEUP_LESSON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_CLASS = new SelectList(db.CLASSROOMs, "ID", "NAME", mAKEUP_LESSON.ID_CLASS);
            ViewBag.ID_ROOM = new SelectList(db.ROOMs, "ID", "NAME_ROM", mAKEUP_LESSON.ID_ROOM);
            ViewBag.ID_SUBJECT = new SelectList(db.SUBJECTs, "ID", "NAME", mAKEUP_LESSON.ID_SUBJECT);
            return View(mAKEUP_LESSON);
        }

        // GET: MAKEUP_LESSONTeacher/Delete/5
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

        // POST: MAKEUP_LESSONTeacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MAKEUP_LESSON mAKEUP_LESSON = db.MAKEUP_LESSON.Find(id);
            db.MAKEUP_LESSON.Remove(mAKEUP_LESSON);
            db.SaveChanges();
            return RedirectToAction("GetApplicaMakeupLessonForTeacher", "Home", new { areas = "" });
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
