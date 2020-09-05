using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Slider : BaseEntity
    {
        [Display(Name = "اولویت")]
        public int Order { get; set; }
        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "عنوان دوم")]
        public string SecondTitle { get; set; }
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        [Display(Name = "توضیحات")]
        public string Body { get; set; }
        [Display(Name = "عنوان لینک")]
        public string LinkTitle { get; set; }
        [Display(Name = "صفحه فرود لینک")]
        public string LinkDestination { get; set; }
    }
}