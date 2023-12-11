using PagedList;
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
    public class NOTIFICATIONsController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var Notifications = db.NOTIFICATIONs.OrderByDescending(m => m.DATESUBMIT).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Notifications.ToPagedList(pageNumber, pageSize));
        }

        // GET: NOTIFICATIONs
        public PartialViewResult ListNotificationsReview()
        {
            return PartialView(db.NOTIFICATIONs.OrderByDescending(m => m.DATESUBMIT).Take(15).ToList());
        }


        // GET: NOTIFICATIONs new date
        public PartialViewResult GetListNotificationsNew()
        {
            return PartialView(db.NOTIFICATIONs.OrderByDescending(m => m.DATESUBMIT).Take(4).ToList());
        }

        // GET: NOTIFICATIONs similar
        public PartialViewResult GetListNotificationsSimilar(int? id)
        {
            Recommend recommend = new Recommend();
            string searchTerm = db.NOTIFICATIONs.Find(id).TITLE;
            return PartialView(db.NOTIFICATIONs.Where(n => n.ID != id).ToList().OrderByDescending(n => recommend.ComputeSimilarity(n.TITLE, searchTerm)).Take(5));
        }

        // GET: NOTIFICATIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            ViewBag.fILE_NOTIFICATIONs = db.FILE_NOTIFICATION.Where(m => m.ID_NOTIFICATION == id).ToList();
            return View(nOTIFICATION);
        }

        // GET: NOTIFICATIONs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NOTIFICATIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TITLE,CONTENT,DATESUBMIT")] NOTIFICATION nOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.NOTIFICATIONs.Add(nOTIFICATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nOTIFICATION);
        }

        // GET: NOTIFICATIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(nOTIFICATION);
        }

        // POST: NOTIFICATIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,TITLE,CONTENT,DATESUBMIT")] NOTIFICATION nOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOTIFICATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nOTIFICATION);
        }

        // GET: NOTIFICATIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(nOTIFICATION);
        }

        // POST: NOTIFICATIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            db.NOTIFICATIONs.Remove(nOTIFICATION);
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
