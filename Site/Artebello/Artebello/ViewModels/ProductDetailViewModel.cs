using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductDetailViewModel: _BaseViewModel
    {
        public ProductGroup ProductGroup { get; set; }
        public Product Product { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductComment> Comments { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}