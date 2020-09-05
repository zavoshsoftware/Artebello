using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class ProductImage:BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "اولویت")]
        public int Priority { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(200)]
        public string ImageUrl { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        internal class Configuration : EntityTypeConfiguration<ProductImage>
        {
            public Configuration()
            {
                HasOptional(p => p.Product)
                    .WithMany(j => j.ProductImages)
                    .HasForeignKey(p => p.ProductId);
            }
        }
    }
}