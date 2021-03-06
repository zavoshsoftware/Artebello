﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductTheme : BaseEntity
    {
        public ProductTheme()
        {
            Products = new List<Product>();
        }
        public string Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
