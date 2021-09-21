
using APPDEVDraft2021.Models;
using PayFast;
using PayFast.AspNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using APPDEVDraft2021.Repository;
namespace APPDEVDraft2021.Controllers
{

    


    public class PayFastAdminController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork(); 
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly PayFastSettings payFastSettings;
        // GET: PayFast


        public PayFastAdminController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = "10022898";
            this.payFastSettings.MerchantKey = "mh16pjyd6tbja";
            this.payFastSettings.PassPhrase = "M431216vees2323";

            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }

        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PayNow()
        {
            string quote = Session["QID"].ToString();
            int quoteid = Int32.Parse(quote);

            //var invoicedet = db.Quotations.FirstOrDefault(x => x.QuoteID == Quoteid);
            //var UserDet = db.Users.FirstOrDefault(x => x.UserID == Userid);
            var custid = db.QuotationTbls.FirstOrDefault(a => a.QuotationID == quoteid);
            int customerID = custid.CustomerID;
            var cartid = db.RecordQuotes.FirstOrDefault(a => a.CustomerID == custid.CustomerID && a.VehicleID == custid.VehicleID);
            string CID = cartid.CartID;
            var customer = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerID);
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            
            // Merchant Details
            onceOffRequest.merchant_id = "10022898";
            onceOffRequest.merchant_key = "mh16pjyd6tbja";
            onceOffRequest.return_url = "https://2021grp05.azurewebsites.net/PayFastAdmin/Return";
            onceOffRequest.cancel_url = "https://2021grp05.azurewebsites.net/PayFastAdmin/Cancel";
            onceOffRequest.notify_url = "https://2021grp05.azurewebsites.net/PayFastAdmin/Notify";


            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            onceOffRequest.amount = (double)custid.QuoteTotal;
            onceOffRequest.name_first = customer.FirstName;
            onceOffRequest.name_last = customer.Surname;
            onceOffRequest.item_name = "Referenced from Quote No:" + quoteid;
            onceOffRequest.item_description = "Vees Tyre and Alignment";

            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";
            

            //addded new

            decimal quotetotal = 0;

            InvoiceTbl ob = new InvoiceTbl();



            ob.QuoteID = quoteid;
            ob.CustomerID = custid.CustomerID;

            string crtid = custid.CartID;

            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == crtid).ToList();




            if (list.Count() < 1)
            {
                List<ReportCart> list1 = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecordsIQueryable().Where(a => a.CartID == crtid).ToList();
                foreach (ReportCart item in list1)
                {
                    quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;



                }
                ob.AmountPaid = quotetotal;

            }
            else
            {
                foreach (QuoteCart item in list)
                {
                    quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;



                }
                ob.AmountPaid = quotetotal;

            }
           
            ob.DateOfInvoice = DateTime.Now;
            ob.PaymentType = "PayFast";



            db.InvoiceTbls.Add(ob);
            db.SaveChanges();

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
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}