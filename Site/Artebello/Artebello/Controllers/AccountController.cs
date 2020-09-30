using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Security;
using ViewModels;
using System.Text.RegularExpressions;
using Models;
using Helpers;
using SmsIrRestful;
using System.Collections.Generic;
using Helpers;

namespace Controllers
{

    public class AccountController : Controller
    {

  


        private DatabaseContext db = new DatabaseContext();
        //MenuHelper menu = new MenuHelper();
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;
            LoginViewModel loginViewModel = new LoginViewModel();
            //loginViewModel.Menu = menu.ReturnMenu();
            //loginViewModel.FooterLink = menu.GetFooterLink();
            //loginViewModel.Username = menu.ReturnUsername();
            return View(loginViewModel);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User oUser = db.Users.Where(a => a.CellNum == model.Username && a.Password == model.Password).FirstOrDefault();

                if (oUser != null)
                {
                    Role role = db.Roles.Find(oUser.RoleId);

                    var ident = new ClaimsIdentity(
                      new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, oUser.CellNum),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,oUser.Id.ToString()),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, role.Name),

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                    //HttpContext.GetOwinContext().Authentication.SignIn(
                    //   new AuthenticationProperties { IsPersistent = true }, ident);
                    //



                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.Now.AddDays(1) }, ident);
                    return RedirectToLocal(returnUrl, role.Name); // auth succeed 
                }
                else
                {
                    // invalid username or password
                    TempData["WrongPass"] = "نام کاربری و یا کلمه عبور وارد شده صحیح نمی باشد.";
                }
            }
            // If we got this far, something failed, redisplay form
            LoginPageViewModel login = new LoginPageViewModel();
            login.login = model;
            LoginViewModel loginViewModel = new LoginViewModel();
            //loginViewModel.Menu = menu.ReturnMenu();
            //loginViewModel.FooterLink = menu.GetFooterLink();
            return View(loginViewModel);

        }

        private ActionResult RedirectToLocal(string returnUrl, string role)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (role.ToLower().Contains("admin"))
                return RedirectToAction("Index", "Users");

            else if (role.ToLower().Contains("seller"))
                return RedirectToAction("Index", "SellerProducts");

            else
                return Redirect("/");
        }
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
            return Redirect("/");
        }



        public ActionResult Register()
        {
            RegisterViewModel register = new RegisterViewModel();
            //register.Menu = menu.ReturnMenu();
            //register.FooterLink = menu.GetFooterLink();
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "registerimage").FirstOrDefault().ImageUrl;
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;
                user.Id = Guid.NewGuid();
                user.RoleId = new Guid("BBCE3864-B441-4E3D-9ED6-6DF036A9D441");
                user.IsActive = true;
                user.Code = RandomCode();
                db.Users.Add(user);
                db.SaveChanges();


                Role role = db.Roles.Find(user.RoleId);

                var ident = new ClaimsIdentity(
                  new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, user.CellNum),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,user.CellNum),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, role.Name),

                  },
                  DefaultAuthenticationTypes.ApplicationCookie);

                //HttpContext.GetOwinContext().Authentication.SignIn(
                //   new AuthenticationProperties { IsPersistent = true }, ident);
                //



                HttpContext.GetOwinContext().Authentication.SignIn(
                   new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.Now.AddDays(1) }, ident);
                return Redirect("/");
            }
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "registerimage").FirstOrDefault().ImageUrl;
            return View(user);
        }
        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }


        [Route("userlogin")]
        public ActionResult UserLogin(string ReturnUrl = "")
        {
            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;

            LoginViewModel login = new LoginViewModel()
            {
                //Menu = menu.ReturnMenu(),
                //FooterLink = menu.GetFooterLink()
            };
            ViewBag.HeaderImage = db.Texts.Where(x => x.TextType.Name == "registerimage").FirstOrDefault().ImageUrl;
            return View(login);
        }

        #region Methods

        [AllowAnonymous]
        public ActionResult SendOtp(string cellNumber)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
                bool isValidMobile = Regex.IsMatch(cellNumber, @"(^(09|9)[0-9][0-9]\d{7}$)|(^(09|9)[3][12456]\d{7}$)", RegexOptions.IgnoreCase);

                if (isValidMobile)
                {
                    User user = db.Users.FirstOrDefault(current => current.CellNum == cellNumber);

                    if (user != null)
                    {

                        SendSms.SendOtpSms(cellNumber, user.Password);

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }


                    return Json("invalidUser", JsonRequestBehavior.AllowGet);

                }
                return Json("invalidCellNumber", JsonRequestBehavior.AllowGet);

                //else
                //{
                //    Guid userId = CreateUser(fullName, cellNumber, email, employeeType);
                //    int codeInt = CreateActivationCode(userId);
                //    code = codeInt.ToString();
                //}


                //UnitOfWork.Save();

            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult CompleteRegister(string cellNumber, string fullName,string isSeller)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                User user = db.Users.FirstOrDefault(current => current.CellNum == cellNumber);

                int code = 0;

                if (user == null)
                {
                    Guid roleId=Guid.NewGuid();
                    if(isSeller == "true")
                    {
                        roleId = new Guid("d7465bc0-e3e3-42d4-b7a3-d914593ab804");
                    }
                    else
                    {
                        roleId = new Guid("bbce3864-b441-4e3d-9ed6-6df036a9d441");

                    }
                    user = CreateUser(fullName, cellNumber, roleId);

                    SendSms.SendOtpSms(user.CellNum, user.Password);

                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                return Json("false", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public int ReturnCode()
        {
            User user = db.Users.OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 300001;
            }
        }

        public User CreateUser(string fullName, string cellNumber, Guid roleId)
        {
            //Guid roleId = new Guid("5812D0C0-264F-4A9B-96BB-42DFC70538E6");

            User user = new User()
            {
                CellNum = cellNumber,
                FullName = fullName,
                RoleId = roleId,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                Code = ReturnCode(),
                IsActive = false,
                Id = Guid.NewGuid(),
                Password = RandomCode().ToString()
            };

            db.Users.Add(user);
            if(roleId == new Guid("d7465bc0-e3e3-42d4-b7a3-d914593ab804"))
            {
                Seller seller = new Seller()
                {
                    UserId = user.Id,
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = false,
                    Id = Guid.NewGuid(),
                    Title = user.FullName
                };
                db.Sellers.Add(seller);
            }

            
            db.SaveChanges();

            return user;
        }

        [AllowAnonymous]
        public ActionResult CheckOtp(string cellNumber, string activationCode)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
                activationCode = activationCode.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                User user = db.Users.FirstOrDefault(current => current.CellNum == cellNumber);

                if (user != null)
                {
                    if (user.Password == activationCode)
                    {
                        user.IsActive = true;
                        user.LastModifiedDate = DateTime.Now;
                        db.SaveChanges();

                        LoginWithOtp(user);
                        if (user.Role.Name.ToLower() == "seller")
                            return Json("true", JsonRequestBehavior.AllowGet);

                        if (user.Role.Name.ToLower() == "administrator" || user.Role.Name.ToLower() == "superadministrator")
                            return Json("true-admin", JsonRequestBehavior.AllowGet);

                        if (user.Role.Name.ToLower() == "customer")
                            return Json("true-customer", JsonRequestBehavior.AllowGet);


                    }
                    return Json("invalid", JsonRequestBehavior.AllowGet);
                }
                return Json("invalid", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public void LoginWithOtp(User oUser)
        {
            var ident = new ClaimsIdentity(
                new[] { 
                    // adding following 2 claim just for supporting default antiforgery provider
                    new Claim(ClaimTypes.NameIdentifier, oUser.CellNum),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                    new Claim(ClaimTypes.Name,oUser.Id.ToString()),

                    // optionally you could add roles if any
                    new Claim(ClaimTypes.Role, oUser.Role.Name),
                    new Claim(ClaimTypes.Surname, oUser.FullName),

                },
                DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(600),

                },
                ident);

        }

       
        #endregion


        public ActionResult ChangeToSeller()
        {
            Guid sellerId = new Guid("D7465BC0-E3E3-42D4-B7A3-D914593AB804");
            Guid userId = new Guid(User.Identity.Name);
            User oUser = db.Users.Find(userId);
            oUser.RoleId = sellerId;

            Seller seller = new Seller()
            {
                UserId = oUser.Id,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                IsActive = false,
                Id = Guid.NewGuid(),
                Title = oUser.FullName
            };
            db.Sellers.Add(seller);
            db.SaveChanges();

            Role role = db.Roles.Find(oUser.RoleId);

            var ident = new ClaimsIdentity(
              new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, oUser.CellNum),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,oUser.Id.ToString()),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, role.Name),

              },
              DefaultAuthenticationTypes.ApplicationCookie);

            //HttpContext.GetOwinContext().Authentication.SignIn(
            //   new AuthenticationProperties { IsPersistent = true }, ident);
            //



            HttpContext.GetOwinContext().Authentication.SignIn(
               new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.Now.AddDays(1) }, ident);
            return RedirectToLocal(null, role.Name); // auth succeed 

        }
    }
}
