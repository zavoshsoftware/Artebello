using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Resources.Models;
using SmsIrRestful;
using ViewModels;
using ActivationCode = Models.ActivationCode;
using DiscountCode = Models.DiscountCode;
using Order = Models.Order;
using Product = Models.Product;
using User = Models.User;

namespace Artebello.Controllers
{

    public class ShopController : Controller
    {
        private BaseViewModelHelper _baseHelper = new BaseViewModelHelper();
        private DatabaseContext db = new DatabaseContext();


        [Route("cart")]
        [HttpPost]
        public ActionResult AddToCart(string code, string qty)
        {
            int productCode = Convert.ToInt32(code);
            int productQuantity = Convert.ToInt32(qty);
            Product product = db.Products.Where(current => current.Code == productCode).FirstOrDefault();
            if(product.IsAvailable && product.Quantity >= productQuantity)
            {
                SetCookie(code, qty);
                ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "shoppingimage").FirstOrDefault().ImageUrl;
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            
            return Json("false", JsonRequestBehavior.AllowGet);
        }


        [Route("Basket")]
        public ActionResult Basket(string qty, string code)
        {
            BasketViewModel cart = new BasketViewModel();

            List<ProductInCart> productInCarts = GetProductInBasketByCoockie();

            cart.Products = productInCarts;

            decimal subTotal = GetSubtotal(productInCarts);

            cart.SubTotal = subTotal.ToString("n0") + " تومان";

            decimal discountAmount = GetDiscount();

            cart.DiscountAmount = discountAmount.ToString("n0") + " تومان";

            cart.Total = (subTotal - discountAmount).ToString("n0");
            cart.Provinces = db.Provinces.OrderBy(current => current.Title).ToList();
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "shoppingimage").FirstOrDefault().ImageUrl;
            return View(cart);
        }



        [AllowAnonymous]
        public ActionResult DiscountRequestPost(string coupon)
        {
            DiscountCode discount = db.DiscountCodes.Where(current => current.Code == coupon).FirstOrDefault();

            string result = CheckCouponValidation(discount);

            if (result != "true")
                return Json(result, JsonRequestBehavior.AllowGet);

            List<ProductInCart> productInCarts = GetProductInBasketByCoockie();
            decimal subTotal = GetSubtotal(productInCarts);

            decimal total = subTotal;

            DiscountHelper helper = new DiscountHelper();

            decimal discountAmount = helper.CalculateDiscountAmount(discount, total);

            SetDiscountCookie(discountAmount.ToString(), coupon);

            return Json("true", JsonRequestBehavior.AllowGet);
        }

        ZarinPalHelper zp = new ZarinPalHelper();

        [AllowAnonymous]
        public ActionResult CheckUser(string email, string cellNumber, string fullName)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                bool isValidMobile = Regex.IsMatch(cellNumber, @"(^(09|9)[0-9][0-9]\d{7}$)|(^(09|9)[3][12456]\d{7}$)", RegexOptions.IgnoreCase);

                if (!isValidMobile)
                    return Json("invalidMobile", JsonRequestBehavior.AllowGet);


                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                    return Json("invalidEmail", JsonRequestBehavior.AllowGet);

                User user = db.Users.FirstOrDefault(current => current.CellNum == cellNumber);

                string code;

                if (user != null)
                {
                    code = user.Password;
                }

                else
                {
                    Guid userId = CreateUser(fullName, cellNumber, email);
                    int codeInt = CreateActivationCode(userId);
                    code = codeInt.ToString();
                }

                db.SaveChanges();

                SendSms(cellNumber, code);

                return Json("true", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        [AllowAnonymous]
        public ActionResult Finalize(string notes, string email, string cellNumber, string activationCode, string city, string address, string postal)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                activationCode = activationCode.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                User user = db.Users.Where(current => current.CellNum == cellNumber).FirstOrDefault();

                if (user != null)
                {
                    ActivationCode activation = IsValidActivationCode(user.Id, activationCode);

                    if (activation != null)
                    {
                        ActivateUser(user, activationCode);
                        UpdateActivationCode(activation, null, null, null, null);
                        db.SaveChanges();

                        List<ProductInCart> productInCarts = GetProductInBasketByCoockie();

                        Order order = ConvertCoockieToOrder(productInCarts, user.Id, notes, email, city, address, postal);

                        RemoveCookie();

                        string res = "";

                        if (order.TotalAmount == 0)
                            res = "freecallback?orderid=" + order.Id;

                        else
                            res = zp.ZarinPalRedirect(order, order.TotalAmount);

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }

                    if (user.IsActive && user.Password == activationCode)
                    {
                        List<ProductInCart> productInCarts = GetProductInBasketByCoockie();

                        Order order = ConvertCoockieToOrder(productInCarts, user.Id, notes, email, city, address, postal);

                        RemoveCookie();

                        string res = "";

                        if (order.TotalAmount == 0)
                            res = "freecallback?orderid=" + order.Id;

                        else
                            res = zp.ZarinPalRedirect(order, order.TotalAmount);

                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("invalid", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActivationCode IsValidActivationCode(Guid userId, string activationCode)
        {
            int code = Convert.ToInt32(activationCode);

            ActivationCode oActivationCode = db.ActivationCodes.Where(current => current.UserId == userId && current.Code == code && current.IsUsed == false
                                && current.IsActive == true && current.ExpireDate > DateTime.Now).FirstOrDefault();

            if (oActivationCode != null)
                return oActivationCode;
            else
                return null;
        }

        public void ActivateUser(User user, string code)
        {
            user.IsActive = true;
            user.LastModifiedDate = DateTime.Now;
            user.Password = code;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateActivationCode(ActivationCode activationCode, string deviceId, string deviceModel,
            string OsType, string OsVersion)
        {
            activationCode.IsUsed = true;
            activationCode.UsingDate = DateTime.Now;
            activationCode.IsActive = false;
            activationCode.LastModifiedDate = DateTime.Now;

            db.Entry(activationCode).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void SendSms(string cellNumber, string code)
        {
            var token = new Token().GetToken("773e6490afdaeccca1206490", "123qwe!@#QWE");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 29580,
                ParameterArray = new List<UltraFastParameters>()
                {
                    new UltraFastParameters()
                    {
                        Parameter = "verifyCode" , ParameterValue = code
                    }
                }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {

            }
        }


        public Guid CreateUser(string fullName, string cellNumber, string email)
        {
            Guid roleId = new Guid("BBCE3864-B441-4E3D-9ED6-6DF036A9D441");

            User user = new User()
            {
                CellNum = cellNumber,
                FullName = fullName,
                Email = email,
                RoleId = roleId,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                Code = ReturnCode(),
                IsActive = false,
                Id = Guid.NewGuid()
            };

            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public int ReturnCode()
        {
            Guid userRoleId = ReturnUserEmployeeRole();
            Guid userEmployerRoleId = ReturnUserEmployerRole();
            User user = db.Users.Where(current => current.RoleId == userRoleId || current.RoleId == userEmployerRoleId).OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 300001;
            }
        }

        public int CreateActivationCode(Guid userId)
        {
            DeactiveOtherActivationCode(userId);

            int code = RandomCode();
            ActivationCode activationCode = new ActivationCode();
            activationCode.UserId = userId;
            activationCode.Code = code;
            activationCode.ExpireDate = DateTime.Now.AddDays(2);
            activationCode.IsUsed = false;
            activationCode.IsActive = true;
            activationCode.CreationDate = DateTime.Now;
            activationCode.IsDeleted = false;

            db.ActivationCodes.Add(activationCode);
            return code;
        }

        public void DeactiveOtherActivationCode(Guid userId)
        {
            List<ActivationCode> activationCodes = db.ActivationCodes.Where(current => current.UserId == userId && current.IsActive == true).ToList();

            foreach (ActivationCode activationCode in activationCodes)
            {
                activationCode.IsActive = false;
                activationCode.LastModifiedDate = DateTime.Now;

            }

        }

        private Random random = new Random();
        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }

        public Guid ReturnUserEmployeeRole()
        {
            Guid roleId = new Guid("6D352C2F-6E64-4762-AAE4-00F49979D7F1");
            return roleId;
        }

        public Guid ReturnUserEmployerRole()
        {
            Guid roleId = new Guid("B999EB27-7330-4062-B81F-62B3D1935885");
            return roleId;
        }
        public void RemoveCookie()
        {
            if (Request.Cookies["basket"] != null)
            {
                Response.Cookies["basket"].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies["discount"] != null)
            {
                Response.Cookies["discount"].Expires = DateTime.Now.AddDays(-1);
            }

        }
        public Order ConvertCoockieToOrder(List<ProductInCart> products, Guid userid, string note, string email, string city, string address, string postal)
        {
            try
            {
                Order order = new Order();


                int expiredNum = 0;
                Guid? cityId = null;

                if (!string.IsNullOrEmpty(city) && city != "0")
                    cityId = new Guid(city);

                order.Id = Guid.NewGuid();
                order.IsActive = true;
                order.IsDeleted = false;
                order.IsPaid = false;
                order.CreationDate = DateTime.Now;
                order.LastModifiedDate = DateTime.Now;
                order.Code = FindeLastOrderCode() + 1;
                order.UserId = userid;
                order.Description = note;
                order.Email = email;
                order.CityId = cityId;
                order.Address = address;
                order.PostalCode = postal;

                decimal subtotal = GetSubtotal(products);

                order.Amount = subtotal;

                order.DiscountAmount = GetDiscount();

                order.TotalAmount = Convert.ToDecimal(subtotal - order.DiscountAmount);


                db.Orders.Add(order);
                db.SaveChanges();
                foreach (ProductInCart product in products)
                {
                    decimal amount = product.Product.Amount;
                    if (product.Product.IsInPromotion && product.Product.DiscountAmount != null)
                    {
                        amount = product.Product.DiscountAmount.Value;
                    }
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        ProductId = product.Product.Id,
                        Quantity = product.Quantity,
                        RawAmount = amount * product.Quantity,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now,
                        OrderId = order.Id,
                        Amount = product.Product.Amount,

                    };

                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();

                }
                return order;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int FindeLastOrderCode()
        {
            Order order = db.Orders.OrderByDescending(current => current.Code).FirstOrDefault();
            if (order != null)
                return order.Code;
            else
                return 999;
        }
        [AllowAnonymous]
        public string CheckCouponValidation(DiscountCode discount)
        {
            if (discount == null)
                return "Invald";

            if (!discount.IsMultiUsing)
            {

            }

            if (discount.ExpireDate < DateTime.Today)
                return "Expired";

            return "true";
        }


        public void SetDiscountCookie(string discountAmount, string discountCode)
        {
            HttpContext.Response.Cookies.Set(new HttpCookie("discount")
            {
                Name = "discount",
                Value = discountAmount + "/" + discountCode,
                Expires = DateTime.Now.AddDays(1)
            });
        }



        public decimal GetDiscount()
        {
            if (Request.Cookies["discount"] != null)
            {
                try
                {
                    string cookievalue = Request.Cookies["discount"].Value;

                    string[] basketItems = cookievalue.Split('/');
                    return Convert.ToDecimal(basketItems[0]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }


        public decimal GetSubtotal(List<ProductInCart> orderDetails)
        {
            decimal subTotal = 0;

            foreach (ProductInCart orderDetail in orderDetails)
            {
                decimal amount = orderDetail.Product.Amount;
                if (orderDetail.Product.IsInPromotion && !string.IsNullOrEmpty(orderDetail.Product.DiscountAmount.ToString()))
                    amount = orderDetail.Product.DiscountAmount.Value;

                subTotal = subTotal + (amount * orderDetail.Quantity);
            }

            return subTotal;
        }

        public List<ProductInCart> GetProductInBasketByCoockie()
        {
            List<ProductInCart> productInCarts = new List<ProductInCart>();

            string[] basketItems = GetCookie();

            if (basketItems != null)
            {
                for (int i = 0; i < basketItems.Length - 1; i++)
                {
                    string[] productItem = basketItems[i].Split('^');

                    int productCode = Convert.ToInt32(productItem[0]);

                    Product product = db.Products.Where(current => current.Code == productCode)
                        .FirstOrDefault();

                    productInCarts.Add(new ProductInCart()
                    {
                        Product = product,
                        Quantity = Convert.ToInt32(productItem[1]),

                    });
                }
            }

            return productInCarts;
        }

        public void SetCookie(string code, string quantity)
        {
            string cookievalue = null;

            if (Request.Cookies["basket"] != null)
            {
                bool changeCurrentItem = false;

                cookievalue = Request.Cookies["basket"].Value;

                string[] coockieItems = cookievalue.Split('/');

                for (int i = 0; i < coockieItems.Length - 1; i++)
                {
                    string[] coockieItem = coockieItems[i].Split('^');

                    if (coockieItem[0] == code)
                    {
                        coockieItem[1] = (Convert.ToInt32(coockieItem[1]) + 1).ToString();
                        changeCurrentItem = true;
                        coockieItems[i] = coockieItem[0] + "^" + coockieItem[1];
                        break;
                    }
                }

                if (changeCurrentItem)
                {
                    cookievalue = null;
                    for (int i = 0; i < coockieItems.Length - 1; i++)
                    {
                        cookievalue = cookievalue + coockieItems[i] + "/";
                    }

                }
                else
                    cookievalue = cookievalue + code + "^" + quantity + "/";

            }
            else
                cookievalue = code + "^" + quantity + "/";

            HttpContext.Response.Cookies.Set(new HttpCookie("basket")
            {
                Name = "basket",
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(1)
            });
        }

        public string[] GetCookie()
        {
            if (Request.Cookies["basket"] != null)
            {
                string cookievalue = Request.Cookies["basket"].Value;

                string[] basketItems = cookievalue.Split('/');

                return basketItems;
            }

            return null;
        }

        public long GetAmountByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
               db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return Convert.ToInt64(zarinpallAuthority.Amount);

            return 0;
        }

        public Order GetOrderByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return zarinpallAuthority.Order;

            else
                return null;
        }

        private String MerchantId = "3f5c16a0-7fe8-11e9-a12a-000c29344814";

        [Route("callback")]
        public ActionResult CallBack(string authority, string status)
        {
            String Status = status;
            CallBackViewModel callBack = new CallBackViewModel();

            if (Status != "OK")
            {
                callBack.IsSuccess = false;
                callBack.RefrenceId = "414";
                Order order = GetOrderByAuthority(authority);
                if (order != null)
                {
                    callBack.Order = order;
                    callBack.OrderDetails = db.OrderDetails
                                .Where(c => c.OrderId == order.Id && c.IsDeleted == false).Include(c => c.Product).ToList();
                }
            }

            else
            {
                try
                {
                    var zarinpal = ZarinPal.ZarinPal.Get();
                    zarinpal.EnableSandboxMode();
                    String Authority = authority;
                    long Amount = GetAmountByAuthority(Authority);

                    var verificationRequest = new ZarinPal.PaymentVerification(MerchantId, Amount, Authority);
                    var verificationResponse = zarinpal.InvokePaymentVerification(verificationRequest);
                    if (verificationResponse.Status == 100 || verificationResponse.Status == 101)
                    {
                        Order order = GetOrderByAuthority(authority);
                        if (order != null)
                        {
                            order.IsPaid = true;
                            order.PaymentDate = DateTime.Now;
                            order.RefId = verificationResponse.RefID;

                            db.SaveChanges();
                            callBack.Order = order;
                            callBack.IsSuccess = true;
                            callBack.OrderCode = order.Code.ToString();
                            callBack.RefrenceId = verificationResponse.RefID;

                            callBack.OrderDetails = db.OrderDetails
                                .Where(c => c.OrderId == order.Id && c.IsDeleted == false).Include(c => c.Product).ToList();
                            foreach (OrderDetail orderDetail in callBack.OrderDetails)
                            {
                                Product product = orderDetail.Product;
                                product.Quantity = orderDetail.Product.Quantity - 1;
                                
                                if (product.Quantity == 0)
                                {
                                    product.IsAvailable = false;
                                }
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            callBack.IsSuccess = false;
                            callBack.RefrenceId = "سفارش پیدا نشد";
                        }
                    }
                    else
                    {
                        callBack.IsSuccess = false;
                        callBack.RefrenceId = verificationResponse.Status.ToString();
                    }
                }
                catch (Exception e)
                {
                    callBack.IsSuccess = false;
                    callBack.RefrenceId = "خطا سیستمی. لطفا با پشتیبانی سایت تماس بگیرید";
                }
            }
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "returnfrombank").FirstOrDefault().ImageUrl;

            return View(callBack);

        }

        [Route("Basket/remove/{code}")]
        public ActionResult RemoveFromBasket(string code)
        {
            string[] coockieItems = GetCookie();


            for (int i = 0; i < coockieItems.Length - 1; i++)
            {
                string[] coockieItem = coockieItems[i].Split('^');

                if (coockieItem[0] == code)
                {
                    string removeArray = coockieItem[0] + "^" + coockieItem[1];
                    coockieItems = coockieItems.Where(current => current != removeArray).ToArray();
                    break;
                }
            }

            string cookievalue = null;
            for (int i = 0; i < coockieItems.Length - 1; i++)
            {
                cookievalue = cookievalue + coockieItems[i] + "/";
            }

            HttpContext.Response.Cookies.Set(new HttpCookie("basket")
            {
                Name = "basket",
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(1)
            });
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "shoppingimage").FirstOrDefault().ImageUrl;

            return RedirectToAction("Basket");
        }

        //public string SendEmailForCustomers()
        //{
        //    List<Order> orders = UnitOfWork.OrderRepository
        //        .Get(current => current.IsSiteOrder == true && current.IsPaid == true).ToList();

        //    string ret = "";

        //    foreach (Order order in orders)
        //    {
        //        string orderType = UnitOfWork.ProductTypeRepository.GetById(order.OrderTypeId).Name;

        //        OrderDetail orderDetail = UnitOfWork.OrderDetailRepository
        //            .Get(current => current.OrderId == order.Id).FirstOrDefault();

        //        if (orderDetail != null)
        //        {
        //            Product product = UnitOfWork.ProductRepository.GetById(orderDetail.ProductId);

        //            if (product != null)
        //            {
        //                string fileLink = "https://ghanongostar.zavoshsoftware.com/" + product.FileUrl;
        //                CreateEmail(order.Email, fileLink, orderType);
        //                ret = ret + order.Email + "/" + fileLink + ".................";
        //            }
        //        }
        //    }

        //    return ret;
        //}



        //  public void CreateEmailForAdmin(string productTitle, long amount, string cellNumber, string address, string city, string postalCode, string fullName)
        //{
        //    Helpers.Message message = new Message();

        //    string email = "hajizadehlaw@yahoo.com";
        //    string email = "hajizadevahid797@gmail.com";
        //    string email = "babaei.aho@gmail.com";
        //    string body = @"<html>
        //         <head></head>
        //        <body dir='rtl'>
        //        <h1> خرید از وب سایت قانون گستر</h1>
        //        <p> از وب سایت قانون گستر خریدی انجام شده است</p>
        //        <p>عنوان کالا: </p>" + productTitle + @"
        //        <p>مبلغ پرداخت شده: </p>" + amount + @"
        //        <h2>دانلود اپلیکیشن قانون گستر</h2>
        //        <p><a href='https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en'>دانلود نسخه اندروید از گوگل پلی</a></p>
        //        <p><a href='https://sibche.com/applications/ghanon-gostar'>دانلود نسخه ios از سیبچه</a></p>
        //        <h3>مجموعه قانون گستر</h3>
        //        <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
        //        <p>تلفن: 02177515152</p>
        //        <p>وب سایت: https://ghanongostar.com/ </p>
        //        </body>
        //        </html> ";

        //    message.Send(email, "خرید از وب سایت قانون گستر", body, "email");
        //}
        //public void CreateEmail(string email, string file, string productType)
        //{
        //    Helpers.Message message = new Helpers.Message();
        //    string body = GetMessageBody(file, productType);

        //    message.Send(email, "خرید از وب سایت قانون گستر", body, "email");
        //}
        //public string GetMessageBody(string file, string productType)
        //{
        //    string body;
        //    if (productType == "forms")
        //        body = @"<html>
        //         <head></head>
        //        <body dir='rtl'>
        //        <h1> خرید از وب سایت قانون گستر</h1>
        //        <p> با تشکر از خرید شما از وب سایت قانون گستر</p>
        //        <p style='font-size:20px; color:red;'> لینک دانلود محصول خریداری شده:<a href= '" + file + @"' > دانلود</a></p>

        //        <h3>مجموعه قانون گستر</h3>
        //        <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
        //        <p>تلفن: 02177515152</p>
        //        <p>وب سایت: https://ghanongostar.com/ </p>
        //        </body>
        //        </html> ";
        //    else if (productType == "course")
        //        body = @"<html>
        //         <head></head>
        //        <body dir='rtl'>
        //        <h1> خرید از وب سایت قانون گستر</h1>
        //        <p> با تشکر از خرید شما از وب سایت قانون گستر</p>
        //        <p style='font-size:20px; color:red;'> لینک دانلود محصول خریداری شده:<a href= '" + file + @"' > دانلود</a></p>

        //        <h3>مجموعه قانون گستر</h3>
        //        <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
        //        <p>تلفن: 02177515152</p>
        //        <p>وب سایت: https://ghanongostar.com/ </p>
        //        </body>
        //        </html> ";

        //    else if (productType == "physicalproduct" || productType == "event" || productType == "workshop")
        //    {
        //        body = @"<html>
        //                     <head></head>
        //        <body dir='rtl'>
        //        <h1> خرید از وب سایت قانون گستر</h1>
        //        <p> با تشکر از خرید شما از وب سایت قانون گستر</p>
        //        <p>همکاران ما جهت هماهنگی با شما تماس خواهند گرفت</p>
        //          <h3>مجموعه قانون گستر</h3>
        //        <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
        //        <p>تلفن: 02177515152</p>
        //        <p>وب سایت: https://ghanongostar.com/ </p>
        //        </body>
        //        </html> ";
        //    }

        //    else
        //    {
        //        body = @"<html>
        //                     <head></head>
        //        <body dir='rtl'>
        //        <h1> خرید از وب سایت قانون گستر</h1>
        //        <p> با تشکر از خرید شما از وب سایت قانون گستر</p>
        //        <h2>دانلود اپلیکیشن قانون گستر</h2>
        //        <p><a href='https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en'>دانلود نسخه اندروید از گوگل پلی</a></p>
        //        <p><a href='https://sibche.com/applications/ghanon-gostar'>دانلود نسخه ios از سیبچه</a></p>
        //        <h3>مجموعه قانون گستر</h3>
        //        <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
        //        <p>تلفن: 02177515152</p>
        //        <p>وب سایت: https://ghanongostar.com/ </p>
        //        </body>
        //        </html>";
        //    }

        //    return body;
        //}

        public ActionResult FillCities(string id)
        {
            Guid provinceId = new Guid(id);
            //   ViewBag.cityId = ReturnCities(provinceId);
            var cities = db.Cities.Where(c => c.ProvinceId == provinceId).OrderBy(current => current.Title).ToList();
            List<CityItemViewModel> cityItems = new List<CityItemViewModel>();
            foreach (Models.City city in cities)
            {
                cityItems.Add(new CityItemViewModel()
                {
                    Text = city.Title,
                    Value = city.Id.ToString()
                });
            }
            return Json(cityItems, JsonRequestBehavior.AllowGet);
        }

    }
}