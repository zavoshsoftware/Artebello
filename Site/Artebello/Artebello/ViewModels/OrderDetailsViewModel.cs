using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class OrderDetailsViewModel:_BaseViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}