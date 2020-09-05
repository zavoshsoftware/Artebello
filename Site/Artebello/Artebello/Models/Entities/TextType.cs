using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TextType:BaseEntity
    {
        public TextType()
        {
            Texts = new List<Text>();
        }

        [Display(Name = "عنوان گروه")]
        public string Title { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }


        public virtual ICollection<Text> Texts { get; set; }
    }
}