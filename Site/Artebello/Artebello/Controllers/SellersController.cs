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
using ViewModels;

namespace Artebello.Controllers
{
    [Authorize(Roles = "administrator")]

    public class SellersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Sellers
        public ActionResult Index()
        {
            var sellers = db.Sellers.Include(s => s.User).Where(s=>s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);
            return View(sellers.ToList());
        }

       

        // GET: Sellers/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users.Where(current=>current.IsActive && !current.IsDeleted).ToList(), "Id", "FullName");
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Seller seller,HttpPostedFileBase fileupload,HttpPostedFileBase resumeUpload, HttpPostedFileBase headerUrlUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    seller.ImageUrl = newFilenameUrl;
                }
                if (resumeUpload != null)
                {
                    string filename = Path.GetFileName(resumeUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    resumeUpload.SaveAs(physicalFilename);
                    seller.ResumeUrl = newFilenameUrl;
                }
                if (headerUrlUpload != null)
                {
                    string filename = Path.GetFileName(headerUrlUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    headerUrlUpload.SaveAs(physicalFilename);
                    seller.HeaderUrl = newFilenameUrl;
                }
                #endregion
                seller.IsDeleted=false;
				seller.CreationDate= DateTime.Now; 
                seller.Id = Guid.NewGuid();
                db.Sellers.Add(seller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsActive && !current.IsDeleted).ToList(), "Id", "FullName",seller.UserId);
            return View(seller);
        }

        // GET: Sellers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsActive && !current.IsDeleted).ToList(), "Id", "FullName", seller.UserId);
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Seller seller, HttpPostedFileBase fileupload,HttpPostedFileBase resumeUpload, HttpPostedFileBase headerUrlUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    seller.ImageUrl = newFilenameUrl;
                }
                if (resumeUpload != null)
                {
                    string filename = Path.GetFileName(resumeUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    resumeUpload.SaveAs(physicalFilename);
                    seller.ResumeUrl = newFilenameUrl;
                }
                if (headerUrlUpload != null)
                {
                    string filename = Path.GetFileName(headerUrlUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    headerUrlUpload.SaveAs(physicalFilename);
                    seller.HeaderUrl = newFilenameUrl;
                }
                #endregion
                seller.IsDeleted=false;
                db.Entry(seller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsActive && !current.IsDeleted).ToList(), "Id", "FullName", seller.UserId);
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Seller seller = db.Sellers.Find(id);
			seller.IsDeleted=true;
			seller.DeletionDate=DateTime.Now;
 
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
        [Route("artists")]
        [AllowAnonymous]
        public ActionResult List()
        {
            SellerListViewModel sellerList = new SellerListViewModel()
            {
                Sellers = db.Sellers.Where(current => current.IsDeleted == false && current.IsActive).ToList()
            };
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "artistimage").FirstOrDefault().ImageUrl;
            return View(sellerList);
        }
        [Route("artist/{id:Guid}")]
        [AllowAnonymous]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            SellerDetailViewModel sellerDetail = new SellerDetailViewModel()
            {
                Seller = seller,
                SellerDetail = ReturnSellerDetail(seller)
            };
            return View(sellerDetail);
        }
        [AllowAnonymous]
        public SellerDetail ReturnSellerDetail(Seller seller)
        {
            SellerDetail detail = new SellerDetail();
            List<Product> Products = db.Products.Where(x => x.IsActive && !x.IsDeleted && x.SellerId.Value == seller.Id).ToList();
            List<ProductGroup> productGroups = new List<ProductGroup>();
            foreach (Product product in Products)
            {
                ProductGroup pg = db.ProductGroups.Where(x => x.IsActive && !x.IsDeleted && x.Id == product.ProductGroupId).FirstOrDefault();
                if (!productGroups.Contains(pg))
                {
                    productGroups.Add(pg);
                }
            }
            detail.Products = Products;
            detail.ProductGroups = productGroups;
            return detail;
        }

       
    }
}
