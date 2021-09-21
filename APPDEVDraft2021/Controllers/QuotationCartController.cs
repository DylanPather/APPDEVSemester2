using APPDEVDraft2021.Models;
using APPDEVDraft2021.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APPDEVDraft2021.Controllers
{
    public class QuotationCartController : Controller
    {

        // GET: ShoppingCart
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var cart = QuotationCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new QuoteCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);

        }
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var productadd = db.StockServiceTbls
                .Single(product => product.StockID == id);

            // Add it to the shopping cart
            var cart = QuotationCart.GetCart(this.HttpContext);

            cart.AddToCart(productadd);

            // Go back to the main store page for more shopping
            return RedirectToAction("GenerateQuotation", "Admin");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = QuotationCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string ProductName = db.QuoteCarts
                .Single(item => item.QuoteCartID == id).StockServiceTbl.Name;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new QuoteCartRemoveViewModel
            {
                Message = Server.HtmlEncode(ProductName) +
                    " has been removed from the quotation.",
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
        public ActionResult QuoteSummary()
        {
            var cart = QuotationCart.GetCart(this.HttpContext);

            ViewData["QuoteCartCount"] = cart.GetCount();
            return PartialView("QuoteSummary");
        }
    }

}


