using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Models;
using ViewModels;
using static ViewModels._BaseViewModel;

//using ViewModels;

namespace Helpers
{
    public class BaseViewModelHelper
    {
        private DatabaseContext db = new DatabaseContext();

        public List<MegaMenuProducts> GetMenuProductGroup()
        {
            List<MegaMenuProducts> menuProducts = new List<MegaMenuProducts>();
            List<ProductGroup> productGroups = db.ProductGroups.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            foreach (ProductGroup group in productGroups)
            {
                menuProducts.Add(new MegaMenuProducts()
                {
                    ProductGroup = group,
                    Products = db.Products.Where(current => current.IsActive && !current.IsDeleted && current.ProductGroupId == group.Id).ToList()
                });
            }
            return menuProducts;
        }
        public Text GetFooterAbout()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "aboutfooter").FirstOrDefault();
        }
        public List<Blog> GetFooterBlogs()
        {
            return db.Blogs.Where(c => c.IsActive == true && c.IsDeleted == false).OrderBy(c=>c.CreationDate).Take(3).ToList();
        }
        public Text GetFooterAddress()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "address").FirstOrDefault();
        }
        public Text GetFooterPhone()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "phone").FirstOrDefault();
        }
        public Text GetFooterEmail()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "email").FirstOrDefault();
        }
        public Text GetMegaMenuImage()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "megamenuimage").FirstOrDefault();
        }
        public Text GetMegaMenuImage2()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "megamenuimage2").FirstOrDefault();
        }
        public Text GetFooterImage()
        {
            return db.Texts.Where(c => c.IsActive == true && c.IsDeleted == false && c.TextType.Name == "footerimage").FirstOrDefault();
        }
    }
}