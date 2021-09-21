using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayFast;
using PayFast.AspNet;
using APPDEVDraft2021.Models;
using APPDEVDraft2021.ViewModels;
using System.Configuration;
using System.Data.Entity;

namespace APPDEVDraft2021.Controllers
{
    public class PayFastController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly PayFastSettings payFastSettings;
        // GET: PayFast
        public PayFastController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = "Pass/Key/002";

            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult PayNow()
        {
            var shoppingCart = new ShoppingCartViewModel();
            string Userid = (string)Session["UserID"];
            int Orderid = db.Orders.Max(X=>X.OrderId);
            var invoicedet = db.Orders.Where(x => x.OrderId == Orderid).Select(c=>c.OrderId).SingleOrDefault();
            var UserDet = db.Users.FirstOrDefault(x => x.Email == Userid);
            //decimal total = (decimal)Session["InvoiceTotal"];
            //double total = (double)shoppingCart.CartTotal;
            var price = db.OrderDetails.Where(x => x.OrderId == invoicedet).Select(c => (double)c.UnitPrice).SingleOrDefault();
            var quantity = db.OrderDetails.Where(x => x.OrderId == invoicedet).Select(c => c.Quantity).SingleOrDefault();
            var total = price*quantity;
            //var ctotal = shoppingCart.CartTotal;
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            onceOffRequest.merchant_id = "10022675";
            onceOffRequest.merchant_key = "0caip8f6e5rqm";
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;


            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            onceOffRequest.amount = total;

            onceOffRequest.name_first = db.OrderDetails.Where(x => x.OrderId == invoicedet).Select(c => c.FirstName).SingleOrDefault();
            onceOffRequest.name_last = db.OrderDetails.Where(x => x.OrderId == invoicedet).Select(c => c.LastName).SingleOrDefault();
            onceOffRequest.item_name = "Referenced from Quote No:" + invoicedet;
            onceOffRequest.item_description = db.OrderDetails.Where(x=>x.OrderId==invoicedet).Select(c=>c.Name).SingleOrDefault();

            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

                 // update order totals
            int max1 = db.OrderDetails.Max(p => p.OrderDetailId);
            int Lastod = db.OrderDetails.Where(p => p.OrderDetailId == max1).Select(p => p.OrderId).SingleOrDefault();
            var orderdetails = new OrderDetails();
            orderdetails.OrderDetailId = max1;
            orderdetails.OrderId = Lastod;
            orderdetails.FirstName = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.FirstName).SingleOrDefault();
            orderdetails.LastName = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.LastName).SingleOrDefault();
            orderdetails.Address = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Address).SingleOrDefault();
            orderdetails.City = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.City).SingleOrDefault();
            orderdetails.State = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.State).SingleOrDefault();
            orderdetails.PostalCode = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.PostalCode).SingleOrDefault();
            orderdetails.Country = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Country).SingleOrDefault();
            orderdetails.Phone = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Phone).SingleOrDefault();
            orderdetails.Email = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Email).SingleOrDefault();
            orderdetails.ProductId = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.ProductId).SingleOrDefault();
            orderdetails.Name = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Product.PName).SingleOrDefault();
            orderdetails.Genre = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Product.ProdType).SingleOrDefault();
            orderdetails.Description = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Product.Description).SingleOrDefault();
            orderdetails.Price = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.UnitPrice).SingleOrDefault() * db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Quantity).SingleOrDefault();
            orderdetails.ImageLocation = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Product.ImageLocation).SingleOrDefault();
            orderdetails.Quantity = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.Quantity).SingleOrDefault();
            orderdetails.UnitPrice = db.OrderDetails.Where(p => p.OrderId == Lastod).Select(p => p.UnitPrice).SingleOrDefault();
            TryUpdateModel(orderdetails);
            db.Entry(orderdetails).State = EntityState.Modified;
            db.SaveChanges();

            // stock deduction
            int a = 1;
            if (a == 1)
            {
                int LastOrder = db.OrderDetails.Where(p => p.OrderDetailId == max1).Select(p => p.ProductId).SingleOrDefault();
                var ProdName2 = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.PName).SingleOrDefault();
                var ProdType2 = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.ProdType).SingleOrDefault();
                var ProdPrice2 = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.Price).SingleOrDefault();
                var prodQuan2 = db.OrderDetails.Where(p => p.OrderDetailId == max1).Select(p => p.Quantity).SingleOrDefault();
                var Prodstock2 = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.Stock).SingleOrDefault();


                var product1 = new Product();
                product1.ProductID = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.ProductID).SingleOrDefault();
                product1.PName = ProdName2;
                product1.ProdType = ProdType2;
                product1.Description = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.Description).SingleOrDefault();
                product1.Price = ProdPrice2;
                product1.ImageLocation = db.ProductDetails.Where(p => p.ProductID == LastOrder).Select(p => p.ImageLocation).SingleOrDefault();
                product1.Stock = db.ProductDetails.Where(p => p.ProductID == LastOrder).Max(p => p.Stock)- db.OrderDetails.Where(p => p.OrderDetailId == max1).Select(p => p.Quantity).SingleOrDefault();

                TryUpdateModel(product1);
                db.Entry(product1).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Redirect(redirectUrl);
        }


        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, System.Net.IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}