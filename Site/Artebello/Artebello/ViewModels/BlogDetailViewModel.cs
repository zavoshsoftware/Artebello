using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogDetailViewModel:_BaseViewModel
    {
        public BlogCategory BlogCategory { get; set; }
        public Blog Blog { get; set; }
        public BlogDetailContent BlogDetailContent { get; set; }
    }
    public class BlogDetailContent
    {
        public string BlogContent { get; set; }
        public string BlogMore { get; set; }
    }
}