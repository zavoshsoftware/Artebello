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

    public class BlogsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Blogs
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.BlogCategory).Where(b => b.IsDeleted == false).OrderByDescending(b => b.CreationDate);
            return View(blogs.ToList());
        }



        // GET: Blogs/Create
        public ActionResult Create()
        {
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadHeader, HttpPostedFileBase fileUploadHeaderUrl)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize image if needed
                if (fileUploadHeader != null)
                {
                    string filename = Path.GetFileName(fileUploadHeader.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileUploadHeader.SaveAs(physicalFilename);
                    blog.HeaderImageUrl = newFilenameUrl;
                }
                if (fileUploadHeaderUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadHeaderUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileUploadHeaderUrl.SaveAs(physicalFilename);
                    blog.HeaderUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted = false;
                blog.CreationDate = DateTime.Now;
                blog.Id = Guid.NewGuid();
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title", blog.BlogCategoryId);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title", blog.BlogCategoryId);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadHeader, HttpPostedFileBase fileUploadHeaderUrl)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize image if needed
                if (fileUploadHeader != null)
                {
                    string filename = Path.GetFileName(fileUploadHeader.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileUploadHeader.SaveAs(physicalFilename);
                    blog.HeaderImageUrl = newFilenameUrl;
                }
                if (fileUploadHeaderUrl != null)
                {
                    string filename = Path.GetFileName(fileUploadHeaderUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileUploadHeaderUrl.SaveAs(physicalFilename);
                    blog.HeaderUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted = false;
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title", blog.BlogCategoryId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Blog blog = db.Blogs.Find(id);
            blog.IsDeleted = true;
            blog.DeletionDate = DateTime.Now;

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
        [Route("blog")]
        public ActionResult List()
        {
            List<BlogCategory> BlogCategories = db.BlogCategories.Where(x => x.IsActive && !x.IsDeleted).ToList();
            BlogListViewModel blogListViewModel = new BlogListViewModel()
            {
                BlogCategories = BlogCategories,
                Blogs = db.Blogs.Where(x => x.IsActive && !x.IsDeleted).OrderByDescending(x => x.Order).ToList()
            };
            string blogCategoryList = string.Empty;
            foreach (BlogCategory blogCategory in BlogCategories)
            {
                blogCategoryList += blogCategory.Id.ToString() + " ";
            }
            ViewBag.ProductGroups = blogCategoryList;
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "blogimage").FirstOrDefault().ImageUrl;

            return View(blogListViewModel);
        }

        [AllowAnonymous]
        [Route("blog/{urlParam}")]
        public ActionResult Details(string urlParam)
        {
            if (string.IsNullOrEmpty(urlParam))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Where(x => x.UrlParam == urlParam && x.IsActive && !x.IsDeleted).FirstOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            BlogDetailViewModel blogDetailViewModel = new BlogDetailViewModel()
            {
                Blog = blog,
                BlogCategory = db.BlogCategories.Where(x => x.Id == blog.BlogCategoryId).FirstOrDefault(),
                BlogDetailContent = ReturnBlogDetailContent(blog)
            };

            return View(blogDetailViewModel);
        }

        [AllowAnonymous]
        public BlogDetailContent ReturnBlogDetailContent(Blog blog)
        {
            BlogDetailContent content = new BlogDetailContent();
            string[] contentSplit = blog.Body.Split(new string[] { "<p>" }, StringSplitOptions.None);
            bool isFirst = true;
            foreach (var item in contentSplit)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (isFirst == true)
                    {
                        isFirst = false;
                        content.BlogContent = "<p>" + item;
                    }
                    else
                    {
                        content.BlogMore += "<p>" + item;
                    }
                }
            }
            return content;
        }
    }
}
