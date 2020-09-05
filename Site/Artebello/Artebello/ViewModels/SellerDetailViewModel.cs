using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class SellerDetailViewModel:_BaseViewModel
    {
        public Seller Seller { get; set; }
        public SellerDetail SellerDetail { get; set; }

    }
    public class SellerDetail
    {
        public List<ProductGroup> ProductGroups { get; set; }
        public List<Product> Products { get; set; }
    }
}