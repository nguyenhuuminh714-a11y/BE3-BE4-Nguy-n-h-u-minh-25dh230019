using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BE2__ADMINLAYOUT.Models;

namespace BE2__ADMINLAYOUT.Area.Admin.Controler
{
    public class ProductController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Product
        public ActionResult Index(string searchString, string sortOrder,
            int page = 1)
        {
            int pageSize = 10;

            var products = db.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchString) ||
                    p.ProductDescription.Contains(searchString) ||
                    p.Category.CategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;

                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;

                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            int totalItems = products.Count();

            var result = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Page = page;
            ViewBag.TotalPages =
                (int)System.Math.Ceiling(totalItems / (double)pageSize);

            return View(result);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription,ProductPrice,ProductImage")]
            Products product,
            HttpPostedFileBase UploadImg)
        {
            if (UploadImg != null && UploadImg.ContentLength > 0)
            {
                string folder = Server.MapPath("~/Content/images/");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(UploadImg.FileName);
                string fullPath = Path.Combine(folder, fileName);

                UploadImg.SaveAs(fullPath);
                product.ProductImage = "~/Content/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName",
                product.CategoryID);

            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName",
                product.CategoryID);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription,ProductPrice,ProductImage")]
            Products product,
            HttpPostedFileBase UploadImg)
        {
            var existing = db.Products.Find(product.ProductID);

            if (existing == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                existing.CategoryID = product.CategoryID;
                existing.ProductName = product.ProductName;
                existing.ProductDescription = product.ProductDescription;
                existing.ProductPrice = product.ProductPrice;

                if (UploadImg != null && UploadImg.ContentLength > 0)
                {
                    string folder = Server.MapPath("~/Content/images/");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(UploadImg.FileName);
                    UploadImg.SaveAs(Path.Combine(folder, fileName));

                    existing.ProductImage = "~/Content/images/" + fileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName",
                product.CategoryID);

            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            db.Products.Remove(product);
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