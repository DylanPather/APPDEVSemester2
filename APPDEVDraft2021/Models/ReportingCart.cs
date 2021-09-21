using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVDraft2021.Controllers;
namespace APPDEVDraft2021.Models
{
    public class ReportingCart
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string ReportingCartid { get; set; }
        public const string CartSessionKey = "QuoteCartId";
        public static ReportingCart GetCart(HttpContextBase context)
        {
            var cart = new ReportingCart();
            cart.ReportingCartid = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ReportingCart GetCart(ReportCartController controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(StockServiceTbl product)
        {
            // Get the matching cart and stock/service item
            var cartItem = db.ReportCarts.SingleOrDefault(
                c => c.CartID == ReportingCartid
                && c.StockID == product.StockID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new ReportCart
                {
                    StockID = product.StockID,
                    SellingPrice = product.SellingPrice,
                    CartID = ReportingCartid,
                    Quantity = 1,
                    CreatedDate = DateTime.Now
                };
                db.ReportCarts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then its add one to the quantity ( Adjustment of quantity via text fields 2nd semester)
                cartItem.Quantity++;
            }
            // Save changes
            db.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.ReportCarts.Single(
                cart => cart.CartID == ReportingCartid
                && cart.ReportCartID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    db.ReportCarts.Remove(cartItem);
                }
                // Saves changes to cart in db
                db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = db.ReportCarts.Where(
                cart => cart.CartID == ReportingCartid);

            foreach (var cartItem in cartItems)
            {
                db.ReportCarts.Remove(cartItem);
            }
            // Save changes to cart in db
            db.SaveChanges();
        }
        public List<ReportCart> GetCartItems()
        {
            return db.ReportCarts.Where(
                cart => cart.CartID == ReportingCartid).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.ReportCarts
                          where cartItems.CartID == ReportingCartid
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {

            decimal? total = (from cartItems in db.ReportCarts
                              where cartItems.CartID == ReportingCartid
                              select (int?)cartItems.Quantity *
                              cartItems.StockServiceTbl.SellingPrice).Sum();

            return total ?? decimal.Zero;
        }


        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public string MechUserID(HttpContextBase context)
        {
            return context.User.Identity.Name.ToString();
        }



    }
}