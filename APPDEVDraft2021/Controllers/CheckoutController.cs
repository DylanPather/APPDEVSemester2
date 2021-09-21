using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVDraft2021.Models;
using APPDEVDraft2021.ViewModels;

namespace APPDEVDraft2021.Controllers
{
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //const string PromoCode = "FREE";
        // GET: Checkout
        public ActionResult AddressAndPayment()
        {
         
            return View();
        }
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            
            var order = new Order();
           
            TryUpdateModel(order);

            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(order);
                }
                //if (string.Equals(values["PromoCode"], PromoCode,
                //    StringComparison.OrdinalIgnoreCase) == false)
                //{
                //    return View(order);
                //}
                else
                {
                order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order

                    db.Orders.Add(order);
                    db.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    //return RedirectToAction("Complete",
                    //   new { id = order.OrderId });
                    return RedirectToAction("PayNow","PayFast");
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}