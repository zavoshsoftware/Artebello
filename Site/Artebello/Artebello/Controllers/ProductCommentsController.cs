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

    public class ProductCommentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductComments
        public ActionResult Index(Guid id)
        {
            var productComments = db.ProductComments.Include(p => p.Product).Where(p=>p.IsDeleted==false && p.ProductId == id).OrderByDescending(p=>p.CreationDate);
            return View(productComments.ToList());
        }

       

        // GET: ProductComments/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title");
            ViewBag.Id = id;

            return View();
        }

        // POST: ProductComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Message,Response,ProductId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductComment productComment)
        {
            if (ModelState.IsValid)
            {
				productComment.IsDeleted=false;
				productComment.CreationDate= DateTime.Now; 
                productComment.Id = Guid.NewGuid();
                db.ProductComments.Add(productComment);
                db.SaveChanges();
                return RedirectToAction("Index",new {id = productComment.ProductId });
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", productComment.ProductId);
            ViewBag.Id = productComment.ProductId;

            return View(productComment);
        }

        // GET: ProductComments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductComment productComment = db.ProductComments.Find(id);
            if (productComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", productComment.ProductId);
            ViewBag.Id = productComment.ProductId;

            return View(productComment);
        }

        // POST: ProductComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Message,Response,ProductId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ProductComment productComment)
        {
            if (ModelState.IsValid)
            {
				productComment.IsDeleted=false;
                db.Entry(productComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = productComment.ProductId });
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", productComment.ProductId);
            ViewBag.Id = productComment.ProductId;

            return View(productComment);
        }

        // GET: ProductComments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductComment productComment = db.ProductComments.Find(id);
            if (productComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = productComment.ProductId;

            return View(productComment);
        }

        // POST: ProductComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductComment productComment = db.ProductComments.Find(id);
			productComment.IsDeleted=true;
			productComment.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = productComment.ProductId });
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
