using APPDEVDraft2021.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVDraft2021.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace APPDEVDraft2021.Controllers
{
    public class MechanicController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private ApplicationDbContext db = new ApplicationDbContext();
        private string cartID;
        public List<SelectListItem> GetBays()
        {

            List<SelectListItem> baylist = new List<SelectListItem>();
            var cust = _unitOfWork.GetRepositoryInstance<WorkshopBayTbl>().GetAllRecords().Where(a => a.IsAvailable == true && a.BayType == "Check Up Bay");
            foreach (var item in cust)
            {
                baylist.Add(new SelectListItem { Value = item.BayID.ToString(), Text = item.BayID.ToString() });
            }
            return baylist;
        }

        public string GetMechID(HttpContextBase context)
        {
            ApplicationUser ob = new ApplicationUser();

            string userid = context.User.Identity.Name;
            return userid;
        }

        
        public ActionResult Dashboard()
        {
            MechanicDashboardCard ob = new MechanicDashboardCard();
            var workbay = _unitOfWork.GetRepositoryInstance<WorkshopBayTbl>().GetAllRecords();
            int baycount = workbay.Count();
            ob.NoWorkBaysAvailable = baycount;

            string name = User.Identity.GetUserId();
            var mechid = db.MechanicTbls.FirstOrDefault(a => a.UserID == name.ToString());
            var mechanicreports = _unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid.MechanicID);

            int reportcount = mechanicreports.Count();
            ob.ReportsMade = reportcount;
            //  var det = _unitOfWork.GetRepositoryInstance<DeliveryDetail>().GetAllRecords();
            //  int detcount = det.Count();
            //  ob.DeliveriesCount = detcount;

            //  var invoice = _unitOfWork.GetRepositoryInstance<Invoice>().GetAllRecords();
            //   int invcount = invoice.Count();
            //   ob.InvoiceCount = invcount;

            //   var appbook = _unitOfWork.GetRepositoryInstance<Tbl_Appointment>().GetAllRecordsIQueryable().Where(a => a.AppointmentStatus == "Pending");
            //   int appcount = appbook.Count();
            //   ob.AppointmentBooking = appcount;


            return View(ob);
        }

        public ActionResult GenerateReport()
        {   
           return View(_unitOfWork.GetRepositoryInstance<StockServiceTbl>().GetProduct());
        }

        public ActionResult AddDetails()
        {

            ViewBag.BayList = GetBays();
            return View();
        }

        [HttpPost]
        public ActionResult AddDetails(ReportTbl report)
        {
            var mcart = new ReportingCart();

            string name = User.Identity.GetUserId();
            var mechid = db.MechanicTbls.FirstOrDefault(a => a.UserID == name.ToString());
            var cartid = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecords();
            var cid = cartid.LastOrDefault();
            string CartID = cid.CartID;
            report.MechanicID = mechid.MechanicID;
            report.CartID = CartID;


            report.DateCreated = DateTime.Now;
            report.Status = "Pending";

            db.ReportTbls.Add(report);
            db.SaveChanges();
            return RedirectToAction("CommitReport");
        }

        public ActionResult CommitReport()
        {
            ReportDetailTbl ob = new ReportDetailTbl();
            ReportTbl obj = new ReportTbl();

            var cartid = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecords();
            var cid = cartid.LastOrDefault();
            string CartID = cid.CartID;
            var getlast = db.ReportTbls.ToArray().LastOrDefault();
            string ID = getlast.ReportID.ToString();
            int ReportID = Int32.Parse(ID);
            List<ReportCart> list = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CartID).ToList();

            foreach (ReportCart item in list)
            {
                ReportDetailTbl reportDetail = new ReportDetailTbl()
                {
                    ReportID = ReportID,
                    StockID = item.StockID,
                    Quantity = item.Quantity
                    

                };

                db.ReportDetailTbls.Add(reportDetail);
                db.SaveChanges();
            }

            return RedirectToAction("ViewReports");
        }

        public ActionResult ViewReports()
        {
            string name = User.Identity.GetUserId();
            var mechid = db.MechanicTbls.FirstOrDefault(a => a.UserID == name.ToString());
            var mechanicreports = _unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid.MechanicID);
            return View(mechanicreports);
        }

        public ActionResult ViewBookedVehicles()
        {
            string name = User.Identity.GetUserId();
            var mechid = db.MechanicTbls.FirstOrDefault(a => a.UserID == name.ToString());
            var mechanicbookedvehicles = _unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid.MechanicID && a.Status == "Booked");
            return View(mechanicbookedvehicles);
        }

        public ActionResult ViewCurrentWork()
        {
            string name = User.Identity.GetUserId();
            var mechid = db.MechanicTbls.FirstOrDefault(a => a.UserID == name.ToString());
            var mechaniccurrentwork = _unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid.MechanicID && a.Status == "CheckedIn" || a.Status == "Working");
            return View(mechaniccurrentwork);
        }

        public ActionResult CheckOut(int bookingid)
        {
            int bayid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.BookingTbls.Where(yy => yy.BookingID == bookingid).ToList();
                foreach (var item in inv)
                {
                    item.Status = "CheckedOut";
                    item.CheckOut = true;
                    item.DateCheckedOut = DateTime.Now;
                    dbs.SaveChanges();
                    bayid = item.BayID;
                }
            }
            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.WorkshopBayTbls.Where(yy => yy.BayID == bayid).ToList();
                foreach (var item in inv)
                {
                    item.IsAvailable = true;
                    dbs.SaveChanges();
                }
            }
            return RedirectToAction("ViewCurrentWork");
        }

        public ActionResult StartWork(int bookingid)
        {
            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.BookingTbls.Where(yy => yy.BookingID == bookingid).ToList();
                foreach (var item in inv)
                {
                    item.Status = "Working";
                    
                    dbs.SaveChanges();
                }
            }
            return RedirectToAction("ViewCurrentWork");
        }


    }
}