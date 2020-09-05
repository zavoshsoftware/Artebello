using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class Product : BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetail>();
            ProductImages = new List<ProductImage>();
            ProductComments = new List<ProductComment>();
        }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }

        
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Product))]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }
        
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Product))]
        [Column(TypeName = "Money")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Amount { get; set; }

        [Display(Name = "درباره اثر")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        [Display(Name = "ImageUrl", ResourceType = typeof(Resources.Models.Product))]
        public string ImageUrl { get; set; }

        [Display(Name = "تصویر Header")]
        public string HeaderUrl { get; set; }

        //[Display(Name="طول")]
        //public int? Width { get; set; }

        //[Display(Name="عرض")]
        //public int? Height { get; set; }
        [Display(Name = "ابعاد (ارتفاع/ طول/ عرض)")]
        public string Dimensions { get; set; }

        [Display(Name = "متریال")]
        public string Material { get; set; }

        [Display(Name= "وزن")]
        public int? Weight { get; set; }

        [Display(Name = "موجودی")]
        public int? Quantity { get; set; }

        [Display(Name="سال خلق اثر")]
        public int CreateYear { get; set; }

        public Guid? SellerId { get; set; }
        public virtual Seller Seller { get; set; }

        [Display(Name = "شاخه هنری")]
        public Guid? ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
        [Display(Name ="موضوع")]
        public Guid? ProductThemeId { get; set; }
        public virtual ProductTheme ProductTheme { get; set; }
        [Display(Name = "تکنیک")]
        public Guid? ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        [Display(Name = "سبک")]
        public Guid? ProductMediumId { get; set; }
        public virtual ProductMedium ProductMedium { get; set; }
        [Display(Name = "رنگ")]
        public Guid? ProductColorId { get; set; }
        public virtual ProductColor ProductColor { get; set; }
        public Guid? ProductOrientationId { get; set; }
        public virtual ProductOrientation ProductOrientation { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductComment> ProductComments { get; set; }
        internal class configuration : EntityTypeConfiguration<Product>
        {
            public configuration()
            {
                HasOptional(p => p.ProductGroup).WithMany(t => t.Products).HasForeignKey(p => p.ProductGroupId);
                HasOptional(p => p.ProductMedium).WithMany(t => t.Products).HasForeignKey(p => p.ProductMediumId);
                HasOptional(p => p.ProductOrientation).WithMany(t => t.Products).HasForeignKey(p => p.ProductOrientationId);
                HasOptional(p => p.ProductTheme).WithMany(t => t.Products).HasForeignKey(p => p.ProductThemeId);
                HasOptional(p => p.ProductType).WithMany(t => t.Products).HasForeignKey(p => p.ProductTypeId);
                HasOptional(p => p.Seller).WithMany(t => t.Products).HasForeignKey(p => p.SellerId);
            }
        }

        [Display(Name = "قیمت بعد از تخفیف")]
        public decimal? DiscountAmount { get; set; }

        [Display(Name = "در حراج است؟")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool IsInPromotion { get; set; }

        [Display(Name = "در صفحه اصلی سایت است؟")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool IsInHome { get; set; }

        [Display(Name = "توضیحات خلاصه")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [NotMapped]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Order))]
        public string AmountStr
        {
            get { return Amount.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Order))]
        public string DiscountAmountStr
        {
            get
            {
                if(DiscountAmount==null)
                     return Amount.ToString("N0");
                return DiscountAmount.Value.ToString("N0");
            }
        }

    }
}
