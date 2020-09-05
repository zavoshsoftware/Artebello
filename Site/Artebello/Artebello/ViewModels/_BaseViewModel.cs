using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;
using Models;

namespace ViewModels
{
    public class _BaseViewModel
    {
        readonly BaseViewModelHelper _baseViewModelHelper = new BaseViewModelHelper();
        public List<MegaMenuProducts> MenuProductGroups
        {
            get { return _baseViewModelHelper.GetMenuProductGroup(); }
        }
        public Text MegaMenuImage
        {
            get { return _baseViewModelHelper.GetMegaMenuImage(); }
        }
        public Text MegaMenuImage2
        {
            get { return _baseViewModelHelper.GetMegaMenuImage2(); }
        }
        public Text FooterImage
        {
            get { return _baseViewModelHelper.GetFooterImage(); }
        }
        public Text FooterAbout
        {
            get { return _baseViewModelHelper.GetFooterAbout(); }
        }
        public List<Blog> FooterBlogs
        {
            get { return _baseViewModelHelper.GetFooterBlogs(); }
        }
        public Text FooterAddress
        {
            get { return _baseViewModelHelper.GetFooterAddress(); }
        }
        public Text FooterPhone
        {
            get { return _baseViewModelHelper.GetFooterPhone(); }
        }
        public Text FooterEmail
        {
            get { return _baseViewModelHelper.GetFooterEmail(); }
        }
        public class MegaMenuProducts
        {
            public ProductGroup ProductGroup { get; set; }
            public List<Product> Products { get; set; }
        }
    }


}