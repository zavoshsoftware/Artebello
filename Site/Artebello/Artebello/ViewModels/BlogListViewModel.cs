using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogListViewModel:BasketViewModel
    {
        public List<BlogCategory> BlogCategories { get; set; }
        public List<Blog> Blogs { get; set; }

    }
}