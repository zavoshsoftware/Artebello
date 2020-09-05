using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BlogCategory : BaseEntity
    {
        public BlogCategory()
        {
            Blogs = new List<Blog>();
        }
        [Display(Name = "عنوان گروه")]
        public string Title { get; set; }

        [Display(Name = "اولویت")]
        public int Order { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
