using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Artebello.Controllers
{
    [Authorize(Roles = "administrator")]

    public class ProductColorsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductColors
        public ActionResult Index()
        {
            return View(db.ProductColors.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ProductColors/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }

        // GET: ProductColors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductColors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,HexCode,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
				productColor.IsDeleted=false;
				productColor.CreationDate= DateTime.Now; 
                productColor.Id = Guid.NewGuid();
                db.ProductColors.Add(productColor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productColor);
        }

        // GET: ProductColors/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }

        // POST: ProductColors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,HexCode,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
				productColor.IsDeleted=false;
                db.Entry(productColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productColor);
        }

        // GET: ProductColors/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }

        // POST: ProductColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductColor productColor = db.ProductColors.Find(id);
			productColor.IsDeleted=true;
			productColor.DeletionDate=DateTime.Now;
 
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
