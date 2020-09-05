using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Artebello.Controllers
{
    [Authorize(Roles = "administrator")]

    public class OrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.City).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate).Include(o => o.User).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        [AllowAnonymous]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            OrderDetailsViewModel viewModel = new OrderDetailsViewModel()
            {
                Order = order,
                OrderDetails = db.OrderDetails.Include(current => current.Product).Where(current => current.OrderId == order.Id && current.IsDeleted == false && current.IsActive).ToList()
            };
            return View(viewModel);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,UserId,Amount,TotalAmount,DiscountAmount,CityId,Address,PostalCode,Email,IsPaid,PaymentDate,RefId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Order order)
        {
            if (ModelState.IsValid)
            {
				order.IsDeleted=false;
				order.CreationDate= DateTime.Now; 
                order.Id = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", order.CityId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", order.CityId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,UserId,Amount,TotalAmount,DiscountAmount,CityId,Address,PostalCode,Email,IsPaid,PaymentDate,RefId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Order order)
        {
            if (ModelState.IsValid)
            {
				order.IsDeleted=false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", order.CityId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
			order.IsDeleted=true;
			order.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult List()
        {
            Guid userId = new Guid(User.Identity.Name);
            OrderListViewModel viewModel = new OrderListViewModel()
            {
                Orders = db.Orders.Where(o=>o.UserId == userId && o.IsActive && !o.IsDeleted).ToList()
            };
            return View(viewModel);
        }
        [Route("order/{code:int}")]
        [AllowAnonymous]
        public ActionResult UserDetails(int code)
        {
            Order order = db.Orders.Where(c=>c.IsActive && !c.IsDeleted && c.Code==code).FirstOrDefault();
            OrderDetailsViewModel viewModel = new OrderDetailsViewModel()
            {
                Order = order,
                OrderDetails = db.OrderDetails.Include(current => current.Product).Where(current => current.OrderId == order.Id && current.IsDeleted == false && current.IsActive).ToList()
            };
            return View(viewModel);
        }
    }
}
