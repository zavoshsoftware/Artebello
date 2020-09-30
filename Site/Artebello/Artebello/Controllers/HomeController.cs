using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Artebello.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Home
        public ActionResult Index()
        {
            HomeViewModel home=new HomeViewModel();
            home.Sliders = db.Sliders.Where(current => current.IsDeleted == false && current.IsActive == true).OrderByDescending(current => current.Order).ToList();
            home.About = db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "homeabout").FirstOrDefault();
            home.LatestProducts = db.Products.Where(current => current.IsActive && !current.IsDeleted && current.IsInHome).OrderByDescending(current => current.CreationDate).ToList();
            home.Sellers = db.Sellers.Where(current => current.IsActive && !current.IsDeleted).ToList();
            home.BlogCategories = db.BlogCategories.Where(current => current.IsActive && !current.IsDeleted).ToList();
            home.Blogs = db.Blogs.Where(current => current.IsActive && !current.IsDeleted).ToList();
            home.MiddleBanner = db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "homemiddlebanner").FirstOrDefault();
            home.AboveSellers = db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "abovesellers").FirstOrDefault();
            home.ProductGroups = db.ProductGroups.Where(current => current.IsActive && !current.IsDeleted && current.IsInHome).Take(4).ToList();
            home.MiddelLink= db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "middlelinks").FirstOrDefault();
            string blogCategoryList = string.Empty;
            foreach (BlogCategory blogCategory in home.BlogCategories)
            {
                blogCategoryList += blogCategory.Id.ToString() + " ";
            }
            ViewBag.ProductGroups = blogCategoryList;
            return View(home);
        }
        public ActionResult ProductList()
        {
            HomeViewModel home=new HomeViewModel();
            return View(home);
        }
        [Route("about")]
        public ActionResult About()
        {
            AboutViewModel about = new AboutViewModel()
            {
                About = db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "mainabout").FirstOrDefault(),
                Titr = db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "aboutservicetitr").FirstOrDefault(),
                Services = db.Texts.Where(current => current.IsActive && !current.IsDeleted && current.TextType.Name == "aboutservice").ToList()
            };
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "aboutimage").FirstOrDefault().ImageUrl;
            return View(about);
        }

        public ActionResult JoinNewsLetter(string email)
        {
            if (ModelState.IsValid)
            {
                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                    return Json("InvalidEmail", JsonRequestBehavior.AllowGet);
                else
                {
                    try
                    {
                        NewsLetter newsLetter = new NewsLetter();
                        newsLetter.Id = Guid.NewGuid();
                        newsLetter.Email = email;
                        newsLetter.IsActive = true;
                        newsLetter.IsDeleted = false;
                        newsLetter.CreationDate = DateTime.Now;

                        db.NewsLetters.Add(newsLetter);
                        db.SaveChanges();

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
                return Json("false", JsonRequestBehavior.AllowGet);
        }


    }
}