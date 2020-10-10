using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViewModels
{
    public class SellerProfileViewModel
    {
        
        [Display(Name = "نام و نام خانوادگی")]
        public string Fullname { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "محل تولد")]
        public string BirthLocation { get; set; }

        [Display(Name = "جنسیت")]
        public bool? Gender { get; set; }

        [Display(Name = "تلفن ثابت")]
        public string Phone { get; set; }

        [Display(Name = "کد پستی")]
        public string ZipCode { get; set; }

        [Display(Name = "استان محل سکونت")]
        public string CurrentProvince { get; set; }

        [Display(Name = "آدرس پستی")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "موبایل")]
        public string CellNum { get; set; }
        public Guid UserId { get; set; }



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
        public Guid SellerId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string Title { get; set; }

        [Display(Name = "عکس هنرمند")]
        public string ImageUrl { get; set; }

        [Display(Name = "رزومه")]
        public string ResumeUrl { get; set; }

        [Display(Name = "سال شروع فعالیت")]
        public int? StartDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "بیوگرافی هنرمند (به صورت مختصر)")]
        public string Summery { get; set; }

        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        [Display(Name = "توضیحات")]
        public string Body { get; set; }
    }
}