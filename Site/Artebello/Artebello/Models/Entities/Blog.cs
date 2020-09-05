using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace Models
{
    public class Blog:BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "اولویت")]
        public int Order { get; set; }

        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        [Display(Name = "توضیحات")]
        public string Body { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "تصویر صفحه داخلی")]
        public string HeaderImageUrl { get; set; }

        [Display(Name = "تصویر Header")]
        public string HeaderUrl { get; set; }

        [Display(Name ="پارامتر url")]
        public string UrlParam { get; set; }

        [Display(Name = "گروه")]
        public Guid BlogCategoryId { get; set; }
        public virtual BlogCategory BlogCategory { get; set; }

        internal class configuration : EntityTypeConfiguration<Blog>
        {
            public configuration()
            {
                HasRequired(p => p.BlogCategory).WithMany(t => t.Blogs).HasForeignKey(p => p.BlogCategoryId);
            }
        }
    }
}
