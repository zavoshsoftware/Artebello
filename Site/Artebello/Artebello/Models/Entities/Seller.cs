using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Seller : BaseEntity
    {
        public Seller()
        {
            Products = new List<Product>();
        }
        [Display(Name = "نام هنرمند")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }
        [Display(Name = "تصویر Header")]
        public string HeaderUrl { get; set; }

        [Display(Name = "رزومه")]
        public string ResumeUrl { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "بیوگرافی هنرمند (به صورت مختصر)")]
        public string Summery { get; set; }

        [Display(Name = "سال شروع فعالیت")]
        public int? StartDate { get; set; }

        [Display(Name = "تحصیلات")]
        public string Education { get; set; }

        [Display(Name = "زمینه هنری")]
        public string ArtField { get; set; }

        [Display(Name = "سابقه شرکت در نمایشگاه داخلی")]
        public bool ParticipatingDomesticExhibitions { get; set; }

        [Display(Name = "سابقه شرکت در نمایشگاه خارجی")]
        public bool ParticipatingForeignExhibitions { get; set; }
        
        [Display(Name = "نحوه آشنایی")]
        public string MethodOfIntroduction { get; set; }

        [Display(Name = "کاربر")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        internal class configuration : EntityTypeConfiguration<Seller>
        {
            public configuration()
            {
                HasRequired(p => p.User).WithMany(j => j.Sellers).HasForeignKey(p => p.UserId);
            }
        }
    }
}