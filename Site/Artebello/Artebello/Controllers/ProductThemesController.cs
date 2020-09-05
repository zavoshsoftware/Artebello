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

    public class ProductThemesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductThemes
        public ActionResult Index()
        {
            return View(db.ProductThemes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ProductThemes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTheme productTheme = db.ProductThemes.Find(id);
            if (productTheme == null)
            {
                return HttpNotFound();
            }
            return View(productTheme);
        }

        // GET: ProductThemes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductThemes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductTheme productTheme)
        {
            if (ModelState.IsValid)
            {
				productTheme.IsDeleted=false;
				productTheme.CreationDate= DateTime.Now; 
                productTheme.Id = Guid.NewGuid();
                db.ProductThemes.Add(productTheme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productTheme);
        }

        // GET: ProductThemes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTheme productTheme = db.ProductThemes.Find(id);
            if (productTheme == null)
            {
                return HttpNotFound();
            }
            return View(productTheme);
        }

        // POST: ProductThemes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductTheme productTheme)
        {
            if (ModelState.IsValid)
            {
				productTheme.IsDeleted=false;
                db.Entry(productTheme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productTheme);
        }

        // GET: ProductThemes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTheme productTheme = db.ProductThemes.Find(id);
            if (productTheme == null)
            {
                return HttpNotFound();
            }
            return View(productTheme);
        }

        // POST: ProductThemes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductTheme productTheme = db.ProductThemes.Find(id);
			productTheme.IsDeleted=true;
			productTheme.DeletionDate=DateTime.Now;
 
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
