using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVDraft2021.Controllers;

namespace APPDEVDraft2021.Models
{
    public class QuotationCart
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string QuotatationCartid { get; set; }
        public const string CartSessionKey = "QuoteCartId";
        public static QuotationCart GetCart(HttpContextBase context)
        {
            var cart = new QuotationCart();
            cart.QuotatationCartid = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static QuotationCart GetCart(QuotationCartController controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(StockServiceTbl product)
        {
            // Get the matching cart and stock/service item
            var cartItem = db.QuoteCarts.SingleOrDefault(
                c => c.CartID == QuotatationCartid
                && c.StockID == product.StockID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new QuoteCart
                {
                    StockID = product.StockID,
                    SellingPrice = product.SellingPrice,
                    CartID = QuotatationCartid,
                    Quantity = 1,
                    CreatedDate = DateTime.Now
                };
                db.QuoteCarts.Add(cartItem);
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
            var cartItem = db.QuoteCarts.Single(
                cart => cart.CartID == QuotatationCartid
                && cart.QuoteCartID == id);

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
                    db.QuoteCarts.Remove(cartItem);
                }
                // Saves changes to cart in db
                db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = db.QuoteCarts.Where(
                cart => cart.CartID == QuotatationCartid);

            foreach (var cartItem in cartItems)
            {
                db.QuoteCarts.Remove(cartItem);
            }
            // Save changes to cart in db
            db.SaveChanges();
        }
        public List<QuoteCart> GetCartItems()
        {
            return db.QuoteCarts.Where(
                cart => cart.CartID == QuotatationCartid).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.QuoteCarts
                          where cartItems.CartID == QuotatationCartid
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            
            decimal? total = (from cartItems in db.QuoteCarts
                              where cartItems.CartID == QuotatationCartid
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
        
       

    }
}