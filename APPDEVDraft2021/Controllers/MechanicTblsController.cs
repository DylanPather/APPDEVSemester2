using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APPDEVDraft2021.Models;

namespace APPDEVDraft2021.Controllers
{
    public class MechanicTblsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MechanicTbls
        public ActionResult Index()
        {
            return View(db.MechanicTbls.ToList());
        }

        // GET: MechanicTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MechanicTbl mechanicTbl = db.MechanicTbls.Find(id);
            if (mechanicTbl == null)
            {
                return HttpNotFound();
            }
            return View(mechanicTbl);
        }

        // GET: MechanicTbls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MechanicTbls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MechanicID,UserID,IsAvailable,FirstName,LastName,ContactNo")] MechanicTbl mechanicTbl)
        {
            if (ModelState.IsValid)
            {
                db.MechanicTbls.Add(mechanicTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mechanicTbl);
        }

        // GET: MechanicTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MechanicTbl mechanicTbl = db.MechanicTbls.Find(id);
            if (mechanicTbl == null)
            {
                return HttpNotFound();
            }
            return View(mechanicTbl);
        }

        // POST: MechanicTbls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MechanicID,UserID,IsAvailable,FirstName,LastName,ContactNo")] MechanicTbl mechanicTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mechanicTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mechanicTbl);
        }

        // GET: MechanicTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MechanicTbl mechanicTbl = db.MechanicTbls.Find(id);
            if (mechanicTbl == null)
            {
                return HttpNotFound();
            }
            return View(mechanicTbl);
        }

        // POST: MechanicTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MechanicTbl mechanicTbl = db.MechanicTbls.Find(id);
            db.MechanicTbls.Remove(mechanicTbl);
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
