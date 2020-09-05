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

    public class ProductMediumsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductMediums
        public ActionResult Index()
        {
            return View(db.ProductMediums.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ProductMediums/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMedium productMedium = db.ProductMediums.Find(id);
            if (productMedium == null)
            {
                return HttpNotFound();
            }
            return View(productMedium);
        }

        // GET: ProductMediums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductMediums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductMedium productMedium)
        {
            if (ModelState.IsValid)
            {
				productMedium.IsDeleted=false;
				productMedium.CreationDate= DateTime.Now; 
                productMedium.Id = Guid.NewGuid();
                db.ProductMediums.Add(productMedium);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productMedium);
        }

        // GET: ProductMediums/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMedium productMedium = db.ProductMediums.Find(id);
            if (productMedium == null)
            {
                return HttpNotFound();
            }
            return View(productMedium);
        }

        // POST: ProductMediums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductMedium productMedium)
        {
            if (ModelState.IsValid)
            {
				productMedium.IsDeleted=false;
                db.Entry(productMedium).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productMedium);
        }

        // GET: ProductMediums/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMedium productMedium = db.ProductMediums.Find(id);
            if (productMedium == null)
            {
                return HttpNotFound();
            }
            return View(productMedium);
        }

        // POST: ProductMediums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductMedium productMedium = db.ProductMediums.Find(id);
			productMedium.IsDeleted=true;
			productMedium.DeletionDate=DateTime.Now;
 
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
