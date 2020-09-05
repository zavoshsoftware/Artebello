using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class HomeViewModel:_BaseViewModel
    {
        public List<Slider> Sliders { get; set; }
        public Text About { get; set; }
        public List<Product> LatestProducts { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
        public List<Blog> Blogs { get; set; }
        public Text MiddleBanner { get; set; }
        public Text AboveSellers { get; set; }
        public List<ProductGroup> ProductGroups { get; set; }
        public Text MiddelLink { get; set; }

    }
}