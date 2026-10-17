using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BE2__ADMINLAYOUT.Area.Admin.Controler
{
    public class CategoriesController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public static object Modified { get; private set; }

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

           CategoriesController category = db.Categories.Find(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "CategoryID,CategoryName")] CategoriesController category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         CategoriesController category = db.Categories.Find(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "CategoryID,CategoryName")] CategoriesController category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = CategoriesController.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CategoriesController category = db.Categories.Find(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriesController category = db.Categories.Find(id);

            if (category == null)
                return HttpNotFound();

            db.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}