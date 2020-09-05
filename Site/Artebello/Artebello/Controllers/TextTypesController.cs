using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Artebello.Controllers
{
    [Authorize(Roles = "administrator")]

    public class TextTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: TextTypes
        public ActionResult Index()
        {
            return View(db.TextType.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: TextTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextType textType = db.TextType.Find(id);
            if (textType == null)
            {
                return HttpNotFound();
            }
            return View(textType);
        }

        // GET: TextTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TextType textType)
        {
            if (ModelState.IsValid)
            {
				textType.IsDeleted=false;
				textType.CreationDate= DateTime.Now; 
                textType.Id = Guid.NewGuid();
                db.TextType.Add(textType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(textType);
        }

        // GET: TextTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextType textType = db.TextType.Find(id);
            if (textType == null)
            {
                return HttpNotFound();
            }
            return View(textType);
        }

        // POST: TextTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TextType textType)
        {
            if (ModelState.IsValid)
            {
				textType.IsDeleted=false;
                db.Entry(textType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(textType);
        }

        // GET: TextTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextType textType = db.TextType.Find(id);
            if (textType == null)
            {
                return HttpNotFound();
            }
            return View(textType);
        }

        // POST: TextTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TextType textType = db.TextType.Find(id);
			textType.IsDeleted=true;
			textType.DeletionDate=DateTime.Now;
 
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
