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
    [Authorize(Roles = "seller")]
    public class SellerOrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SellerOrders
        public ActionResult Index()
        {
            Guid userId = new Guid(User.Identity.Name);
            Seller seller = db.Sellers.Where(s => s.UserId == userId && !s.IsDeleted).FirstOrDefault();
            List<OrderDetail> orderDetails = db.OrderDetails.Include(current=>current.Order).Where(c => c.IsActive && !c.IsDeleted && c.Product.SellerId == seller.Id).ToList();
            List<Order> orders = new List<Order>();
            foreach (OrderDetail orderDetail in orderDetails)
            {
                if(!orders.Contains(orderDetail.Order))
                {
                    orders.Add(orderDetail.Order);
                }
            }
            //var orders = db.Orders.Include(o => o.City).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate).Include(o => o.User).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.CreationDate);
            return View(orders);
        }

        // GET: SellerOrders/Details/5
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
            Guid userId = new Guid(User.Identity.Name);
            Seller seller = db.Sellers.Where(s => s.UserId == userId && s.IsActive && !s.IsDeleted).FirstOrDefault();
            OrderDetailsViewModel viewModel = new OrderDetailsViewModel()
            {
                Order = order,
                OrderDetails = db.OrderDetails.Include(current=>current.Product).Where(current=>current.OrderId == order.Id && current.IsDeleted==false && current.IsActive && current.Product.SellerId == seller.Id).ToList()
            };
            return View(viewModel);
        }

        // GET: SellerOrders/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: SellerOrders/Create
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

        // GET: SellerOrders/Edit/5
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

        // POST: SellerOrders/Edit/5
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

        // GET: SellerOrders/Delete/5
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

        // POST: SellerOrders/Delete/5
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
    }
}
