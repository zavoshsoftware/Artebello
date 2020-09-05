using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Eshop.Helpers
{
    public static class CodeCreator
    {
        private static DatabaseContext db = new DatabaseContext();
        public static int ReturnUserCode()
        {

            User user = db.Users.OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 1000;
            }
        }

        public static int ReturnOrderCode()
        {

            Order order = db.Orders.OrderByDescending(current => current.Code).FirstOrDefault();
            if (order != null)
            {
                return order.Code + 1;
            }
            else
            {
                return 30000;
            }
        }

        public static int ReturnProductCode()
        {

            Product product = db.Products.OrderByDescending(current => current.Code).FirstOrDefault();
            if (product != null)
            {
                return product.Code + 1;
            }
            else
            {
                return 1000;
            }
        }

    }
}