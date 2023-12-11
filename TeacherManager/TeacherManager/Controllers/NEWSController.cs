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
    public class NEWSController : Controller
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var News = db.NEWS.OrderByDescending(m => m.DATESUBMIT).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.image = db.IMAGES_NEW.Where(m => m.ID_IMAGES_NEW_TYPE == 1).ToList();
            return View(News.ToPagedList(pageNumber, pageSize));
        }

        // GET: NEWS
        public PartialViewResult ListNewsReview()
        {
            ViewBag.image = db.IMAGES_NEW.Where(m => m.ID_IMAGES_NEW_TYPE == 1).ToList();
            return PartialView(db.NEWS.OrderByDescending(m => m.DATESUBMIT).Take(15).ToList());
        }


        // GET: NEWS newdate
        public PartialViewResult GetListNewsNew()
        {
            return PartialView(db.NEWS.OrderByDescending(m => m.DATESUBMIT).Take(4).ToList());
        }

        // GET: NEWS similar
        public PartialViewResult GetListNewsSimilar(int? id)
        {
            Recommend recommend = new Recommend();
            string searchTerm = db.NEWS.Find(id).TITLE;
            return PartialView(db.NEWS.Where(n => n.ID != id).ToList().OrderByDescending(n => recommend.ComputeSimilarity(n.TITLE, searchTerm)).Take(5));
        }
       
        // GET: NEWS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS nEWS = db.NEWS.Find(id);
            if (nEWS == null)
            {
                return HttpNotFound();
            }
            ViewBag.image = db.IMAGES_NEW.Where(n => n.ID_NEWS == id).ToList();
            return View(nEWS);
        }

        // GET: NEWS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NEWS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TITLE,CONTENT,DATESUBMIT")] NEWS nEWS)
        {
            if (ModelState.IsValid)
            {
                db.NEWS.Add(nEWS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nEWS);
        }

        // GET: NEWS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS nEWS = db.NEWS.Find(id);
            if (nEWS == null)
            {
                return HttpNotFound();
            }
            return View(nEWS);
        }

        // POST: NEWS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,TITLE,CONTENT,DATESUBMIT")] NEWS nEWS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nEWS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nEWS);
        }

        // GET: NEWS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS nEWS = db.NEWS.Find(id);
            if (nEWS == null)
            {
                return HttpNotFound();
            }
            return View(nEWS);
        }

        // POST: NEWS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NEWS nEWS = db.NEWS.Find(id);
            db.NEWS.Remove(nEWS);
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
