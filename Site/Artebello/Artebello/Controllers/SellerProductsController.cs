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
using Eshop.Helpers;
using ViewModels;

namespace Artebello.Controllers
{
    [Authorize(Roles = "seller")]
    public class SellerProductsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SellerProducts
        public ActionResult Index()
        {
            Guid userId = new Guid(User.Identity.Name);
            var products = db.Products.Include(p => p.ProductColor).Where(p=>p.IsDeleted==false && p.Seller.UserId == userId).OrderByDescending(p=>p.CreationDate).Include(p => p.ProductGroup).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate).Include(p => p.ProductMedium).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate).Include(p => p.ProductOrientation).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate).Include(p => p.ProductTheme).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate).Include(p => p.ProductType).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate).Include(p => p.Seller).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate);
            return View(products.ToList());
        }

        // GET: SellerProducts/Details/5
        public ActionResult Details(Guid? id)
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

        // GET: SellerProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title");
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title");
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title");
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title");
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title");
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title");
            //ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title");
            return View();
        }

        // POST: SellerProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product,HttpPostedFileBase fileupload)
        {
            Guid userId = new Guid(User.Identity.Name);
            Seller seller = db.Sellers.Where(current => current.UserId == userId).FirstOrDefault();
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
                    product.ImageUrl = newFilenameUrl;
                }
                #endregion
                product.Code = CodeCreator.ReturnUserCode();
                product.IsDeleted=false;
                product.IsActive = false;
                product.IsInHome = false;
				product.CreationDate= DateTime.Now; 
                product.Id = Guid.NewGuid();
                product.Seller = seller;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title", product.ProductColorId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title", product.ProductMediumId);
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title", product.ProductOrientationId);
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title", product.ProductThemeId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title", product.ProductTypeId);
            //ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title", product.SellerId);
            return View(product);
        }

        // GET: SellerProducts/Edit/5
        public ActionResult Edit(Guid? id)
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
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title", product.ProductColorId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title", product.ProductMediumId);
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title", product.ProductOrientationId);
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title", product.ProductThemeId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title", product.ProductTypeId);
            //ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title", product.SellerId);
            return View(product);
        }

        // POST: SellerProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product,HttpPostedFileBase fileupload)
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
                    product.ImageUrl = newFilenameUrl;
                }
                #endregion
                Guid userId = new Guid(User.Identity.Name);
                Seller seller = db.Sellers.Where(current => current.UserId == userId).FirstOrDefault();
                product.Seller = seller;
                product.IsDeleted=false;
                product.IsActive = false;
                product.IsInHome = false;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title", product.ProductColorId);
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title", product.ProductMediumId);
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title", product.ProductOrientationId);
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title", product.ProductThemeId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title", product.ProductTypeId);
            //ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title", product.SellerId);
            return View(product);
        }

        // GET: SellerProducts/Delete/5
        public ActionResult Delete(Guid? id)
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

        // POST: SellerProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Find(id);
			product.IsDeleted=true;
			product.DeletionDate=DateTime.Now;
 
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
        public ActionResult EditProfile()
        {
            Guid userId = new Guid(User.Identity.Name);
            User user = db.Users.Find(userId);
            Seller seller = db.Sellers.Where(current =>  !current.IsDeleted && current.UserId == userId).FirstOrDefault();

            List<SelectListItem> Gender = new List<SelectListItem>();
            Gender.Add(new SelectListItem() { Text = "مرد", Value = "false" });
            Gender.Add(new SelectListItem() { Text = "زن", Value = "true" });
            ViewBag.Gender = new SelectList(Gender, "Value", "Text", user.Gender);

            SellerProfileViewModel sellerProfile = new SellerProfileViewModel()
            {
                Fullname = user.FullName,
                Email = user.Email,
                Password = user.Password,
                CellNum = user.CellNum,

                BirthDate = user.BirthDate,
                BirthLocation = user.BirthLocation,
                //Gender = user.Gender,
                Phone = user.Phone,
                ZipCode = user.ZipCode,
                CurrentProvince = user.CurrentProvince,
                Address = user.Address,


                Title = seller.Title,
                ImageUrl = seller.ImageUrl,
                StartDate = seller.StartDate,
                Body = seller.Body,
                ResumeUrl = seller.ResumeUrl,

                Education = seller.Education,
                ArtField = seller.ArtField,
                ParticipatingDomesticExhibitions = seller.ParticipatingDomesticExhibitions,
                ParticipatingForeignExhibitions = seller.ParticipatingForeignExhibitions,
                MethodOfIntroduction = seller.MethodOfIntroduction,
                Summery = seller.Summery,

                UserId = user.Id,
                SellerId = seller.Id
            };
            TempData["SizeError"] = "";
            return View(sellerProfile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(SellerProfileViewModel profile, HttpPostedFileBase fileupload,HttpPostedFileBase resumeupload)
        {
            if (ModelState.IsValid)
            {
                Seller seller = db.Sellers.Find(profile.SellerId);
                User user = db.Users.Find(profile.UserId);
                List<SelectListItem> Gender = new List<SelectListItem>();
                Gender.Add(new SelectListItem() { Text = "مرد", Value = "false" });
                Gender.Add(new SelectListItem() { Text = "زن", Value = "true" });
                ViewBag.Gender = new SelectList(Gender, "Value", "Text", profile.Gender);

                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    if (fileupload.ContentLength > 4194304)
                    {
                        TempData["SizeError"] = @"<div class='alert alert-danger'>فایل بارگزاری شده نباید بیشتر از 4 مگابایت باشد</div>";
                        return View(profile);
                    }
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    seller.ImageUrl = newFilenameUrl;
                }
                
                if (resumeupload != null)
                {
                    if (resumeupload.ContentLength > 4194304)
                    {
                        TempData["SizeError"] = @"<div class='alert alert-danger'>فایل بارگزاری شده نباید بیشتر از 4 مگابایت باشد</div>";
                        return View(profile);
                    }
                    string filename = Path.GetFileName(resumeupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seller/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    resumeupload.SaveAs(physicalFilename);
                    seller.ResumeUrl = newFilenameUrl;
                }
                #endregion
                seller.Title = profile.Title;
                seller.StartDate = profile.StartDate;
                seller.Body = profile.Body;
                seller.IsDeleted = false;
                seller.Education = profile.Education;
                seller.ArtField = profile.ArtField;
                seller.ParticipatingDomesticExhibitions = profile.ParticipatingDomesticExhibitions;
                seller.ParticipatingForeignExhibitions = profile.ParticipatingForeignExhibitions;
                seller.MethodOfIntroduction = profile.MethodOfIntroduction;
                seller.Summery = profile.Summery;
                db.Entry(seller).State = EntityState.Modified;

                user.FullName = profile.Fullname;
                user.Email = profile.Email;
                user.Password = profile.Password;
                user.CellNum = profile.CellNum;
                user.IsDeleted = false;
                user.BirthDate = profile.BirthDate;
                user.BirthLocation = profile.BirthLocation;
                user.Gender = profile.Gender;
                user.Phone = profile.Phone;
                user.ZipCode = profile.ZipCode;
                user.CurrentProvince = profile.CurrentProvince;
                user.Address = profile.Address;
                db.Entry(user).State = EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
           

            return View(profile);
        }
    }
}
