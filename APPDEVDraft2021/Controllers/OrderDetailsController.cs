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
    public class OrderDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Order).Include(o => o.Product);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Username");
            ViewBag.ProductId = new SelectList(db.ProductDetails, "ProductID", "PName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetailId,OrderId,FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email,ProductId,Name,Genre,Description,Price,ImageLocation,Quantity,UnitPrice")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Username", orderDetails.OrderId);
            ViewBag.ProductId = new SelectList(db.ProductDetails, "ProductID", "PName", orderDetails.ProductId);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Username", orderDetails.OrderId);
            ViewBag.ProductId = new SelectList(db.ProductDetails, "ProductID", "PName", orderDetails.ProductId);
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailId,OrderId,FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email,ProductId,Name,Genre,Description,Price,ImageLocation,Quantity,UnitPrice")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Username", orderDetails.OrderId);
            ViewBag.ProductId = new SelectList(db.ProductDetails, "ProductID", "PName", orderDetails.ProductId);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult OrdersPage(string searchBy, string search)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (searchBy == "OrderId")
            {
                return View(db.OrderDetails.Where(x => x.Order.OrderId.ToString() == search || search == null).ToList());
            }
            else
            {
                return View(db.OrderDetails.Where(x => x.Order.OrderId.ToString().StartsWith(search) || search == null).ToList());
            }

        }

        public ActionResult DetailsPage(string searchBy, string search)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (searchBy == "Genre")
            {
                return View(db.OrderDetails.Where(x => x.Genre == search || search == null).ToList());
            }
            else
            {
                return View(db.OrderDetails.Where(x => x.Email.StartsWith(search) || search == null).ToList());
            }

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
