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

    public class ProductOrientationsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductOrientations
        public ActionResult Index()
        {
            return View(db.ProductOrientations.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ProductOrientations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOrientation productOrientation = db.ProductOrientations.Find(id);
            if (productOrientation == null)
            {
                return HttpNotFound();
            }
            return View(productOrientation);
        }

        // GET: ProductOrientations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductOrientations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductOrientation productOrientation)
        {
            if (ModelState.IsValid)
            {
				productOrientation.IsDeleted=false;
				productOrientation.CreationDate= DateTime.Now; 
                productOrientation.Id = Guid.NewGuid();
                db.ProductOrientations.Add(productOrientation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productOrientation);
        }

        // GET: ProductOrientations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOrientation productOrientation = db.ProductOrientations.Find(id);
            if (productOrientation == null)
            {
                return HttpNotFound();
            }
            return View(productOrientation);
        }

        // POST: ProductOrientations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductOrientation productOrientation)
        {
            if (ModelState.IsValid)
            {
				productOrientation.IsDeleted=false;
                db.Entry(productOrientation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productOrientation);
        }

        // GET: ProductOrientations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOrientation productOrientation = db.ProductOrientations.Find(id);
            if (productOrientation == null)
            {
                return HttpNotFound();
            }
            return View(productOrientation);
        }

        // POST: ProductOrientations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductOrientation productOrientation = db.ProductOrientations.Find(id);
			productOrientation.IsDeleted=true;
			productOrientation.DeletionDate=DateTime.Now;
 
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
