using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductListViewModel:_BaseViewModel
    {
        public ProductGroup ProductGroup { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductGroupViewModel> SidebarProductGroups { get; set; }
        public List<ProductOrientation> SidebarProductOrientations { get; set; }
        public List<ProductColorViewModel> ProductColors { get; set; }
        public string Url { get; set; }
        public int PageNumber { get; set; }
    }

    public class ProductColorViewModel
    {
        public ProductColor ProductColor { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductGroupViewModel
    {
        public ProductGroup ProductGroup { get; set; }
        public int Quantity { get; set; }
    }
}