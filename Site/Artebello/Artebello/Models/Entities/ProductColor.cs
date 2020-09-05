using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductColor : BaseEntity
    {
        public ProductColor()
        {
            Products = new List<Product>();
        }
        public string Title { get; set; }

        public string HexCode { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
