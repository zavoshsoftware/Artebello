﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductGroup:BaseEntity
    {
        public ProductGroup()
        {
            Products = new List<Product>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroup))]
        public string Title { get; set; }

        [Display(Name = "ImageUrl", ResourceType = typeof(Resources.Models.ProductGroup))]
        public string ImageUrl { get; set; }

        [Display(Name = "تصویر Header")]
        public string HeaderUrl { get; set; }


        [Display(Name="پارامتر وب سایت")]
        public string UrlParam { get; set; }

        [Display(Name ="نمایش در صفحه اصلی؟")]
        public bool IsInHome { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
