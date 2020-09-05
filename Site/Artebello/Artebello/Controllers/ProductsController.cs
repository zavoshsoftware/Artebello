using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eshop.Helpers;
using Models;
using ViewModels;
using System.Text.RegularExpressions;

namespace Artebello.Controllers
{
    [Authorize(Roles = "administrator")]

    public class ProductsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductGroup).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate).Include(p => p.ProductMedium).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate).Include(p => p.ProductOrientation).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate).Include(p => p.ProductTheme).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate).Include(p => p.ProductType).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate).Include(p => p.Seller).Where(p => p.IsDeleted == false).OrderByDescending(p => p.CreationDate);
            return View(products.ToList());
        }

        
        public ActionResult Create()
        {
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title");
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title");
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title");
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title");
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title");
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title");
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase fileupload, HttpPostedFileBase headerUpload)
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

                if (headerUpload != null)
                {
                    string filename = Path.GetFileName(headerUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    headerUpload.SaveAs(physicalFilename);
                    product.HeaderUrl = newFilenameUrl;
                }
                #endregion
                product.Code = CodeCreator.ReturnProductCode();
                product.IsDeleted = false;
                product.CreationDate = DateTime.Now;
                product.Id = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title", product.ProductMediumId);
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title", product.ProductOrientationId);
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title", product.ProductThemeId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title", product.ProductTypeId);
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title", product.ProductColorId);
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title", product.SellerId);
            return View(product);
        }

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
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title", product.ProductMediumId);
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title", product.ProductOrientationId);
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title", product.ProductThemeId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title", product.ProductTypeId);
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title", product.SellerId);
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title", product.ProductColorId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase fileupload,HttpPostedFileBase headerUpload)
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
                if (headerUpload != null)
                {
                    string filename = Path.GetFileName(headerUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    headerUpload.SaveAs(physicalFilename);
                    product.HeaderUrl = newFilenameUrl;
                }
                #endregion
                product.IsDeleted = false;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups, "Id", "Title", product.ProductGroupId);
            ViewBag.ProductMediumId = new SelectList(db.ProductMediums, "Id", "Title", product.ProductMediumId);
            ViewBag.ProductOrientationId = new SelectList(db.ProductOrientations, "Id", "Title", product.ProductOrientationId);
            ViewBag.ProductThemeId = new SelectList(db.ProductThemes, "Id", "Title", product.ProductThemeId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "Title", product.ProductTypeId);
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "Title", product.SellerId);
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Title", product.ProductColorId);
            return View(product);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Find(id);
            product.IsDeleted = true;
            product.DeletionDate = DateTime.Now;

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


        [AllowAnonymous]
        [Route("category/{urlParam}")]
        public ActionResult List(string urlParam, string page,string range,string orientation,string color)
        {
            ProductGroup productGroup =
                db.ProductGroups.FirstOrDefault(current => current.UrlParam == urlParam && current.IsDeleted == false);

            if (productGroup == null)
                return RedirectPermanent("/category");

            string url = "category/" + urlParam;

            List<Product> products = GetProducts(productGroup.Id, page);
            var filter = false;

            if (!string.IsNullOrEmpty(range))
            {
                filter = true;
                int from = Convert.ToInt32(range.Split('/')[0]);
                int to = Convert.ToInt32(range.Split('/')[1]);
                products = products.Where(current => current.Amount > from && current.Amount < to).ToList();
            }
            if(!string.IsNullOrEmpty(orientation))
            {
                filter = true;
                Guid orientationId = new Guid(orientation);
                products = products.Where(current => current.ProductOrientationId == orientationId).ToList();
            }
            if (!string.IsNullOrEmpty(color))
            {
                filter = true;
                products = products.Where(current => current.ProductColor.HexCode == color).ToList();
            }
            if(filter == false)
            {
                if (Request.Cookies["range"] != null)
                {
                    var c = new HttpCookie("range");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                if (Request.Cookies["orientation"] != null)
                {
                    var c = new HttpCookie("orientation");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                if (Request.Cookies["color"] != null)
                {
                    var c = new HttpCookie("color");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
            }

            ProductListViewModel productList = new ProductListViewModel()
            {
                ProductGroup = productGroup,

                Products = products,

                SidebarProductOrientations = db.ProductOrientations.Where(c=>c.IsDeleted==false&&c.IsActive).ToList(),

                SidebarProductGroups = GetSidebarProductGroupList(),

                Url = url,

                ProductColors = GetSidebarProductColorList(),

                PageNumber = GetPageSizeFilter(productGroup.Id)
            };

            //productList.Products = FilterProducts(productList.Products, brandparam, ageRangeparam, url);

            //if (productList.Products == null)
            //    return RedirectPermanent(url);

            return View(productList);
        }

      

        [AllowAnonymous]
        [Route("product/{code:int}")]
        public ActionResult Details(int? code)
        {
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(current=>current.ProductColor).Where(x => x.Code == code && x.IsActive && x.IsDeleted == false).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductDetailViewModel productDetail = new ProductDetailViewModel()
            {
                Product = product,
                ProductGroup = db.ProductGroups.Where(current => current.Id == product.ProductGroupId).FirstOrDefault(),
                ProductImages = db.ProductImages.Where(current => current.ProductId == product.Id && current.IsActive && !current.IsDeleted).OrderBy(current => current.Priority).ToList(),
                Comments = db.ProductComments.Where(current => current.ProductId == product.Id && current.IsActive && !current.IsDeleted).ToList(),
                RelatedProducts = db.Products.Where(current => current.SellerId == product.SellerId && current.IsDeleted == false && current.IsActive == true).Take(3).ToList()
            };
            
            return View(productDetail);
        }


        private readonly int PageSize = 12;

        public List<Product> GetProducts(Guid productGroupId, string page)
        {
            List<Product> products = db.Products
                .Where(c => c.ProductGroupId == productGroupId && c.IsActive && c.IsDeleted == false).ToList();

            int pageNumber = 1;

            if (!string.IsNullOrEmpty(page) && page!="''")
                pageNumber = Convert.ToInt32(page);

            products = products.Skip((pageNumber - 1)* PageSize).Take(PageSize).ToList();

            return products;
        }
        public int GetPageSizeFilter(Guid productGroupId)
        {
            List<Product> products = db.Products
                .Where(c => c.ProductGroupId == productGroupId && c.IsActive && c.IsDeleted == false).ToList();
            int count;
            if (products.Count() % 12 > 0)
                count = products.Count() / 12 + 1;
            else
                count = products.Count() / 12;

            return count;
        }
        public List<ProductColorViewModel> GetSidebarProductColorList()
        {
            List<ProductColorViewModel> productColorItems=new List<ProductColorViewModel>();

            List<ProductColor> productColors =
                db.ProductColors.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            foreach (ProductColor productColor in productColors)
            {
                productColorItems.Add(new ProductColorViewModel()
                {
                    ProductColor = productColor,
                    Quantity = db.Products.Count(c=>c.ProductColorId==productColor.Id&&c.IsActive&&c.IsDeleted==false)
                });
            }

            return productColorItems;
        }

        public List<ProductGroupViewModel> GetSidebarProductGroupList()
        {
            List<ProductGroupViewModel> productGroupItems = new List<ProductGroupViewModel>();

            List<ProductGroup> productGroups =
                db.ProductGroups.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            foreach (ProductGroup productGroup in productGroups)
            {
                productGroupItems.Add(new ProductGroupViewModel()
                {
                    ProductGroup = productGroup,
                    Quantity = db.Products.Count(c => c.ProductGroupId == productGroup.Id && c.IsActive && c.IsDeleted == false)
                });
            }

            return productGroupItems;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostSubmitComment(string name, string email, string body, string id)
        {
            try
            {
                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                    return Json("InvalidEmail", JsonRequestBehavior.AllowGet);

                int productCode = Convert.ToInt32(id);

                Product product = db.Products.FirstOrDefault(c => c.Code == productCode && c.IsDeleted == false);

                if (product == null)
                    return Json("false", JsonRequestBehavior.AllowGet);

                ProductComment comment = new ProductComment();

                comment.Name = name;
                comment.Email = email;
                comment.Message = body;
                comment.CreationDate = DateTime.Now;
                comment.IsDeleted = false;
                comment.Id = Guid.NewGuid();
                comment.ProductId = product.Id;
                comment.IsActive = false;
                comment.CreationDate = DateTime.Now;


                db.ProductComments.Add(comment);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
