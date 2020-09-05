using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using System.IO;

namespace Artebello.Controllers
{
    [Authorize(Roles = "administrator")]

    public class ProductImagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductImages
        public ActionResult Index(Guid id)
        {
            var productImages = db.ProductImages.Include(p => p.Product).Where(p=>p.IsDeleted==false && p.ProductId == id).OrderByDescending(p=>p.CreationDate);
            return View(productImages.ToList());
        }

       
        // GET: ProductImages/Create
        public ActionResult Create(Guid id)
        {
            //ViewBag.ProductId = new SelectList(db.Products, "Id", "Title");
            ViewBag.Id = id;
            ProductImage productImage = new ProductImage();
            productImage.ProductId = id;
            return View(productImage);
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductImage productImage,HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    productImage.ImageUrl = newFilenameUrl;
                }
                #endregion
                productImage.IsDeleted=false;
				productImage.CreationDate= DateTime.Now; 
                productImage.Id = Guid.NewGuid();
                db.ProductImages.Add(productImage);
                db.SaveChanges();
                return RedirectToAction("Index" , new { id = productImage.ProductId });
            }

            //ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", productImage.ProductId);
            ViewBag.Id = productImage.ProductId;
            return View(productImage);
        }

        // GET: ProductImages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", productImage.ProductId);
            ViewBag.Id = productImage.ProductId;
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductImage productImage,HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    productImage.ImageUrl = newFilenameUrl;
                }
                #endregion
                productImage.IsDeleted=false;
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = productImage.ProductId });
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", productImage.ProductId);
            ViewBag.Id = productImage.ProductId;
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = productImage.ProductId;
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
			productImage.IsDeleted=true;
			productImage.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = productImage.ProductId });
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
