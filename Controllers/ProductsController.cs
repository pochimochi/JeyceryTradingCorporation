using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JeyceryTradingCorporation.Models;

namespace JeyceryTradingCorporation.Controllers
{
    public class ProductsController : Controller
    {
        private JTCDBEntities db = new JTCDBEntities();

        // GET: Products
        public ActionResult Index()
        {            
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());           
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Size,Weight,DateCreated,DateModified,CategoryID,CreatedBy,ModifiedBy")] Product product, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                product.CreatedBy = "admin";
                product.DateCreated = DateTime.Now.ToString();


                db.Products.Add(product);
                db.SaveChanges();

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        // Verify that the user selected a file
                        if (file != null && file.ContentLength > 0)
                        {
                            FilePath image = new FilePath();
                            Products_Files imagetbl = new Products_Files();
                           
                            // extract only the fielname
                            var fileName = Path.GetFileName(file.FileName);
                            // TODO: need to define destination
                            var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                            file.SaveAs(path);
                            image.Path = "/Upload/"+fileName;
                            image.CreatedBy = "Admin";
                            image.DateCreated = DateTime.Now.ToString();
                            var images = db.FilePaths.Add(image);
                            imagetbl.FileID = images.ID;
                            imagetbl.ProductID = product.ID;
                            db.Products_Files.Add(imagetbl);
                            db.SaveChanges();

                        }
                    }
                }
                
               
                return View("table", db.Products.ToList());
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        [ChildActionOnly]
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View("Edit", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Size,Weight,DateCreated,DateModified,CategoryID,CreatedBy,ModifiedBy")] Product product, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        // Verify that the user selected a file
                        if (file != null && file.ContentLength > 0)
                        {
                            FilePath image = new FilePath();
                            Products_Files imagetbl = new Products_Files();

                            // extract only the fielname
                            var fileName = Path.GetFileName(file.FileName);
                            // TODO: need to define destination
                            var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                            file.SaveAs(path);
                            image.Path = "/Upload/" + fileName;
                            image.CreatedBy = "Admin";
                            image.DateCreated = DateTime.Now.ToString();
                            var images = db.FilePaths.Add(image);
                            imagetbl.FileID = images.ID;
                            imagetbl.ProductID = product.ID;
                            db.Products_Files.Add(imagetbl);

                        }
                    }
                }

                product.ModifiedBy = "admin";
                product.DateModified = DateTime.Now.ToString();

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return View("table", db.Products.ToList());
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View("EditProduct", product);
        }

        

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return PartialView("Table", db.Products.ToList());
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
