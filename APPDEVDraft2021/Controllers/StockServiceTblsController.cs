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
    public class StockServiceTblsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockServiceTbls
        public ActionResult Index()
        {
            var stockServiceTbls = db.StockServiceTbls.Include(s => s.CategoryTbl);
            return View(stockServiceTbls.ToList());
        }

        // GET: StockServiceTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockServiceTbl stockServiceTbl = db.StockServiceTbls.Find(id);
            if (stockServiceTbl == null)
            {
                return HttpNotFound();
            }
            return View(stockServiceTbl);
        }

        // GET: StockServiceTbls/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.CategoryTbls, "CategoryID", "CategoryName");
            return View();
        }

        // POST: StockServiceTbls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( StockServiceTbl stockServiceTbl ,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    stockServiceTbl.ProductImage = new byte[file.ContentLength];
                    file.InputStream.Read(stockServiceTbl.ProductImage, 0, file.ContentLength);
                }
                stockServiceTbl.CreatedDate = DateTime.Now;
                db.StockServiceTbls.Add(stockServiceTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.CategoryTbls, "CategoryID", "CategoryName", stockServiceTbl.CategoryID);
            return View(stockServiceTbl);
        }

        // GET: StockServiceTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockServiceTbl stockServiceTbl = db.StockServiceTbls.Find(id);
            if (stockServiceTbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.CategoryTbls, "CategoryID", "CategoryName", stockServiceTbl.CategoryID);
            return View(stockServiceTbl);
        }

        // POST: StockServiceTbls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockID,Name,CategoryID,Description,Quantity,IsActive,IsDelete,CreatedDate,ModifiedDate,ProductImage,IsFeatured")] StockServiceTbl stockServiceTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockServiceTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.CategoryTbls, "CategoryID", "CategoryName", stockServiceTbl.CategoryID);
            return View(stockServiceTbl);
        }

        // GET: StockServiceTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockServiceTbl stockServiceTbl = db.StockServiceTbls.Find(id);
            if (stockServiceTbl == null)
            {
                return HttpNotFound();
            }
            return View(stockServiceTbl);
        }

        // POST: StockServiceTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockServiceTbl stockServiceTbl = db.StockServiceTbls.Find(id);
            db.StockServiceTbls.Remove(stockServiceTbl);
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

        public ActionResult ProductAdd()
        {
            ViewBag.CategoryID = new SelectList(db.CategoryTbls, "CategoryID", "CategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult ProductAdd(StockServiceTbl tbl, HttpPostedFileBase file)
        {

            StockServiceTbl ob = new StockServiceTbl();
            if (file != null)
            {
                tbl.ProductImage = new byte[file.ContentLength];
                file.InputStream.Read(tbl.ProductImage, 0, file.ContentLength);
            }
            ob.ProductImage = tbl.ProductImage;
            tbl.CreatedDate = DateTime.Now;
            tbl.ModifiedDate = DateTime.Now;



            db.StockServiceTbls.Add(tbl);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
