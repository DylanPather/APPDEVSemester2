using APPDEVDraft2021.Models;
using APPDEVDraft2021.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APPDEVDraft2021.Controllers
{
    public class ReportCartController : Controller

    {

        // GET: ShoppingCart
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var cart = ReportingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ReportCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);

        }
        public ActionResult AddToCart(int id)
        {
            // Retrieve the service from database
            var productadd = db.StockServiceTbls
                .Single(product => product.StockID == id);

            // Add it to the report cart
            var cart = ReportingCart.GetCart(this.HttpContext);

            cart.AddToCart(productadd);

            // Go back to the selection of services/stock
            return RedirectToAction("GenerateReport", "Mechanic");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ReportingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string ProductName = db.ReportCarts
                .Single(item => item.ReportCartID == id).StockServiceTbl.Name;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ReportCartRemoveViewModel
            {
                Message = Server.HtmlEncode(ProductName) +
                    " has been removed from the report.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult ReportSummary()
        {

            var cart = ReportingCart.GetCart(this.HttpContext);

            ViewData["ReportCartCount"] = cart.GetCount();
            return PartialView("ReportSummary");
        }
    }

}


