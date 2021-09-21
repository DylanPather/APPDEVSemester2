using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APPDEVDraft2021.Models;
using APPDEVDraft2021.Repository;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Web.Helpers;
using System.Net.Mail;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using System.IO;

namespace APPDEVDraft2021.Controllers
{
   [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int customerID;
        private string cartID;
        private int vehid;
        private int quoteid;
        // GET: Admin

        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<CategoryTbl>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryID.ToString(), Text = item.CategoryName });
            }
            return list;
        }

        public List<SelectListItem> GetCustomer()
        {
            List<SelectListItem> custlist = new List<SelectListItem>();
            var cust = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords();
            foreach (var item in cust)
            {
                custlist.Add(new SelectListItem { Value = item.CustomerID.ToString(), Text = item.EmailAddress });
            }
            return custlist;
        }

        public ActionResult GenerateQuotation()
        {
            
            return View(_unitOfWork.GetRepositoryInstance<StockServiceTbl>().GetProduct());
        }

      

        public ActionResult AddCustomerInformation()
        {
            return View(_unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords());
        }

        public ActionResult AddCustomerToQuote(int customerid)
        {
            customerID = customerid;
            ApplicationDbContext ob = new ApplicationDbContext();
            var vehicles = _unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetAllRecordsIQueryable().Where(a => a.CustomerID == customerid);
            cartID = ""+Session["QuotecartId"];
            //cartID = cartid.CartID;
            return View(vehicles);
        }

        public ActionResult SelectVehicle(int vehicleid)
        {
            var details = _unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetAllRecordsIQueryable().Where(a => a.VehicleID == vehicleid);
            

            return View(details);
        }

        public ActionResult CustomerVehicles()
        {
            CustomerVehicle ob = new CustomerVehicle();
            var custveh = _unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetAllRecords();
            List<CustomerVehicle> list = _unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetAllRecords().ToList();
            
            return View(list);
        }

        public ActionResult ProceedQuote(int vehicleid)
        {
            RecordQuote ob = new RecordQuote();
            var custid = db.CustomerVehicles.FirstOrDefault(a => a.VehicleID == vehicleid);
            ob.CustomerID = custid.CustomerID;
            ob.VehicleID = custid.VehicleID;
           
            var qcart = new QuotationCart();
            cartID = qcart.GetCartId(HttpContext);
            ob.CartID = cartID;
            db.RecordQuotes.Add(ob);
            db.SaveChanges();

            

            return RedirectToAction("FinalQuotation");
        }
        //Generates view for Final Quotation with Customer Details(Follows Post to Quotation Generation)
        public ActionResult FinalQuotation()
        {
            FinalQuotation ob = new FinalQuotation();
            var custid = _unitOfWork.GetRepositoryInstance<RecordQuote>().GetAllRecords();
            var Cid = custid.LastOrDefault();

            int customerid = Cid.CustomerID;
            string quotecartid = Cid.CartID;
            var custinfo = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerid);
            ob.CartID = quotecartid;
            ob.CustomerFirstName = custinfo.FirstName;
            ob.CustomerLastName = custinfo.Surname;
            ob.CustomerEmail = custinfo.EmailAddress;
            ob.CustomerCellNo = custinfo.ContactNo;
            
            var vehicles = db.RecordQuotes.FirstOrDefault(x => x.VehicleID == Cid.VehicleID);


            var custveh = db.CustomerVehicles.FirstOrDefault(a => a.VehicleID == vehicles.VehicleID);
            ob.VehicleRegNo = custveh.VehicleRegNo;
            ob.VehicleMake = custveh.MakeOfVehicle;
            ob.VehicleModel = custveh.ModelOfVehicle;
            ob.VehicleMilleage = custveh.Mileage;
            var quoteid = _unitOfWork.GetRepositoryInstance<QuotationTbl>().GetAllRecords();
            var id = quoteid.LastOrDefault();
            ob.QuoteID = id.QuotationID + 1;
            decimal quotetotal = 0;

            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == quotecartid).ToList();
            ob.List = list;
            
            foreach (QuoteCart item in list)
            {
                quotetotal = (item.SellingPrice * item.Quantity) + quotetotal; 

            }

            ob.QuoteTotal = quotetotal;
            ob.QuotationTotalLessVat = quotetotal-(quotetotal*(decimal)0.15);


            ob.DateOfQuote = DateTime.Now;
            return View(ob);
        }
      
        public ActionResult QuotationCapture()
        {
            QuoteDetailTbl obj = new QuoteDetailTbl();
            QuotationTbl ob = new QuotationTbl();
            var custid = _unitOfWork.GetRepositoryInstance<RecordQuote>().GetAllRecords();
            var cid = custid.LastOrDefault();

            string CartID = cid.CartID;
            int customerid = cid.CustomerID;
            int vehid = cid.VehicleID;
            ob.CustomerID = customerid;
            ob.VehicleID = vehid;
            ob.QuoteDate = DateTime.Now;
            ob.IsActive = true;
            ob.IsDelete = false;
            ob.Status = "Pending";

            decimal quotetotal = 0;
            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CartID).ToList();
            foreach (QuoteCart item in list)
            {
                quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;

            }
            ob.CartID = CartID;
            ob.QuoteTotal = quotetotal;
            db.QuotationTbls.Add(ob);
            db.SaveChanges();
            return RedirectToAction("QuoteDetailCapture");
        }

        public ActionResult QuoteDetailCapture()
        {
            QuoteDetailTbl obj = new QuoteDetailTbl();
            QuotationTbl ob = new QuotationTbl();
            var custid = _unitOfWork.GetRepositoryInstance<RecordQuote>().GetAllRecords();
            
            var cid = custid.LastOrDefault();
            var getlast = db.QuotationTbls.ToArray().LastOrDefault();
            string ID = getlast.QuotationID.ToString();
            int QuoteID = Int32.Parse(ID);
            string CartID = cid.CartID;
            int customerid = cid.CustomerID;
            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CartID).ToList();
            foreach (QuoteCart item in list)
            {
                QuoteDetailTbl quoteDetail = new QuoteDetailTbl()
                {
                    QuoteID = QuoteID,
                    StockID = item.StockServiceTbl.StockID,
                    Quantity = item.Quantity,

                };

                db.QuoteDetailTbls.Add(quoteDetail);
                db.SaveChanges();
            }


            return RedirectToAction("CreateDocument");
        }

        public ActionResult CreateDocument()
        {
            QuoteDetailTbl obj = new QuoteDetailTbl();
            QuotationTbl ob = new QuotationTbl();
            var custid = _unitOfWork.GetRepositoryInstance<RecordQuote>().GetAllRecords();
            var cid = custid.LastOrDefault();
            var getlast = db.QuotationTbls.ToArray().LastOrDefault();
            string ID = getlast.QuotationID.ToString();
            int QuoteID = Int32.Parse(ID);
            string CartID = cid.CartID;
            int customerid = cid.CustomerID;
            var custname = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerid);
            string name = custname.FirstName + " " + custname.Surname;
            string email = custname.EmailAddress;
            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CartID).ToList();
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            DataTable datatable = new DataTable();
            PdfLightTable table = new PdfLightTable();
            PdfGraphics graphics = page.Graphics;
            PdfImage image = PdfImage.FromFile(Server.MapPath("~/Images/Vees Tyre and Alignment.jpg"));
            RectangleF bounds = new RectangleF(176, 0, 390, 130);
            page.Graphics.DrawImage(image, bounds);
            PdfBrush solidbrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            graphics.DrawRectangle(solidbrush, bounds);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            PdfTextElement element = new PdfTextElement("Quotation No." + ID.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;

            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentdate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            SizeF textsize = subHeadingFont.MeasureString(currentdate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textsize.Width - 10, result.Bounds.Y);
            graphics.DrawString(currentdate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesroman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            element = new PdfTextElement("Quote To :" + name, timesroman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            PdfPen linepen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startpoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            graphics.DrawLine(linepen, startpoint, endPoint);


            datatable.Columns.Add("Item ID");
            datatable.Columns.Add("Item Name");
            datatable.Columns.Add("Item Price");
            datatable.Columns.Add("Item Quantity");
            datatable.Columns.Add("Line Total");
            int c = 0;
            foreach (QuoteCart pdfitems in list)
            {
                datatable.Rows.Add(new object[] { pdfitems.StockServiceTbl.StockID, pdfitems.StockServiceTbl.Name, pdfitems.StockServiceTbl.SellingPrice, pdfitems.Quantity, (pdfitems.Quantity * pdfitems.StockServiceTbl.SellingPrice) });
                c++;
            }

            PdfGrid pdfgrid = new PdfGrid();
            pdfgrid.DataSource = datatable;
            //pdfgrid.Draw(page, new PointF(10, 10));
            table.DataSource = datatable;
            PdfLightTableBuiltinStyleSettings settings = new PdfLightTableBuiltinStyleSettings();
            settings.ApplyStyleForBandedColumns = true;
            settings.ApplyStyleForBandedRows = true;
            settings.ApplyStyleForFirstColumn = true;
            settings.ApplyStyleForHeaderRow = true;
            settings.ApplyStyleForLastColumn = true;
            settings.ApplyStyleForLastRow = true;
            table.ApplyBuiltinStyle(PdfLightTableBuiltinStyle.ListTable6ColorfulAccent4, settings);




            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = pdfgrid.Headers[0];
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);


            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }


            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));

            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();

            layoutFormat.Layout = PdfLayoutType.Paginate;

            PdfGridLayoutResult gridResult = pdfgrid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);
            //string filepath = "C:/Users/Dylan Pather/Documents/Visual Studio 2015/Projects/GoldPrideDecoreHire/GoldPrideDecoreHire/QuotationPDF";
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            doc.Close(true);

            ms.Position = 0;
            Attachment file = new Attachment(ms, QuoteID + " Quotation.pdf", "application/pdf");


            var fromEmail = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "ikgtqxhwvonejuat";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            MailMessage message = new MailMessage(fromEmail, toEmail);
            message.Body = "Hi thank you for requesting a Quote from us, Please be advised quote is valid only for 14 days from date quoted.";
            message.Subject = "Quotation";
            message.IsBodyHtml = false;
            message.Attachments.Add(file);




            smtp.Send(message);
            //Session.Remove("cart");
            return View("QuoteSuccess");
        }

        public ActionResult QuoteSuccess()
        {
            return View();
        }

        public ActionResult Customers()
        {
          
            return View(_unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords());
        }

        public ActionResult Dashboard()
        {
            DashboardCard ob = new DashboardCard();
            var stock = _unitOfWork.GetRepositoryInstance<StockServiceTbl>().GetAllRecords();
            int countstock = stock.Count();
            ob.StockCount = countstock;
            
            var quote = _unitOfWork.GetRepositoryInstance<QuotationTbl>().GetAllRecords();
             int CountUnassign = quote.Count();
             ob.QuotationCount = CountUnassign;

              var bays = _unitOfWork.GetRepositoryInstance<WorkshopBayTbl>().GetAllRecordsIQueryable().Where(a => a.IsAvailable == true);
               int countbay = bays.Count();
            ob.UnassignedBays = countbay;

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

        public ActionResult Quotations()
        {
            return View(_unitOfWork.GetRepositoryInstance<QuotationTbl>().GetAllRecordsIQueryable().Where(a => a.Status == "Pending" || a.Status == "Accepted").ToList());
        }

        public ActionResult QuotationEdit(int QuoteID)
        {
            return View(_unitOfWork.GetRepositoryInstance<QuotationTbl>().GetFirstorDefault(QuoteID));
        }

        [HttpPost]
        public ActionResult QuotationEdit(QuotationTbl Quotation)
        {
            var q = db.QuotationTbls.FirstOrDefault(x => x.QuotationID == Quotation.QuotationID);
            Quotation.CustomerID = q.CustomerID;

            Quotation.DateModified = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<QuotationTbl>().Update(Quotation);

            return RedirectToAction("Quotations");
        }


        public ActionResult StockService()
        {
            return View(_unitOfWork.GetRepositoryInstance<StockServiceTbl>().GetProduct());
        }
        public ActionResult StockServiceEdit(int productID)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<StockServiceTbl>().GetFirstorDefault(productID));
        }
        [HttpPost]
        public ActionResult StockServiceEdit(StockServiceTbl product)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var d = db.StockServiceTbls.FirstOrDefault(x => x.StockID == product.StockID);
            product.ProductImage = d.ProductImage;
            product.ModifiedDate = DateTime.Now;

            _unitOfWork.GetRepositoryInstance<StockServiceTbl>().Update(product);
            return RedirectToAction("StockService");
        }
        public ActionResult StockServiceAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        [HttpPost]
        public ActionResult StockServiceAdd(StockServiceTbl tbl, HttpPostedFileBase file)
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

            return RedirectToAction("StockService");
        }

        public ActionResult AddCustomerVehicle()
        {

            ViewBag.CustomerList = GetCustomer();


            return View();
        }

        [HttpPost]
        public ActionResult AddCustomerVehicle(CustomerVehicle vehicle, HttpPostedFileBase file)
        {
            CustomerVehicle ob = new CustomerVehicle();
            if (file != null)
            {
                vehicle.VehicleImage = new byte[file.ContentLength];
                file.InputStream.Read(vehicle.VehicleImage, 0, file.ContentLength);
            }

            ob.VehicleImage = vehicle.VehicleImage;
            db.CustomerVehicles.Add(vehicle);
            db.SaveChanges();
            return RedirectToAction("GenerateQuotation");
        }

        public ActionResult ViewVehicle(int vehicleid)
        {
           
            return View(_unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetAllRecordsIQueryable().Where(a => a.VehicleID == vehicleid));
        }

        public ActionResult EditCustomerVehicle(int vehicleid)
        {
            vehid = vehicleid;
            ViewBag.CustomerList = GetCustomer();

            return View(_unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetFirstorDefault(vehicleid));
        }

        [HttpPost]
        public ActionResult EditCustomerVehicle(CustomerVehicle tbl , HttpPostedFileBase file)
        {
            CustomerVehicle ob = new CustomerVehicle();
            var custimage = _unitOfWork.GetRepositoryInstance<CustomerVehicle>().GetFirstorDefault(vehid);
            var d = db.CustomerVehicles.FirstOrDefault(x => x.VehicleID == tbl.VehicleID);

            if (d.VehicleImage == null)
            {
                if (file != null)
                {
                    tbl.VehicleImage = new byte[file.ContentLength];
                    file.InputStream.Read(tbl.VehicleImage, 0, file.ContentLength);
                }
            }
            tbl.CustomerID = d.CustomerID;
            _unitOfWork.GetRepositoryInstance<CustomerVehicle>().Update(tbl);



            return RedirectToAction("CustomerVehicles");
        }

        public ActionResult ViewMechanicReports()
        {
            return View(_unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.Status == "Pending"));
        }

        public ActionResult GenerateQuoteFromReport(string vehicleregno)
        {
            QuotationTbl ob = new QuotationTbl();
            RecordQuote obj = new RecordQuote();
            var vehregno = db.CustomerVehicles.FirstOrDefault(a => a.VehicleRegNo == vehicleregno);
            if (vehregno == null)
            {
                return RedirectToAction("AddCustomer");
            }
            else
            {
                var cust = db.CustomerVehicles.Where(a => a.VehicleRegNo == vehicleregno);
               // var getlastcust = vehregno.ToArray().LastOrDefault();
                var customerid = db.CustomerVehicles.FirstOrDefault(a => a.VehicleRegNo == vehicleregno);
                var reportid = _unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.VehicleRegNo == vehicleregno );
                var getlast = reportid.ToArray().LastOrDefault();
                int custid = vehregno.CustomerID;
                string cardid = getlast.CartID;
                ob.VehicleID = vehregno.VehicleID;
                ob.CustomerID = custid;
                ob.QuoteDate = DateTime.Now;
                ob.IsActive = true;
                ob.IsDelete = false;
                ob.Status = "Pending";
                obj.CustomerID = custid;
                obj.VehicleID = customerid.VehicleID;
                obj.CartID = cardid;
                ob.CartID = cardid;
                decimal quotetotal = 0;
                List<ReportCart> list = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecordsIQueryable().Where(a => a.CartID == cardid).ToList();
                foreach (ReportCart item in list)
                {
                    quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;

                }

                ob.QuoteTotal = quotetotal;
                db.RecordQuotes.Add(obj);
                db.QuotationTbls.Add(ob);
                db.SaveChanges();

                return RedirectToAction("CommitQuoteDetailReport");

            }

            
        }

        public ActionResult CommitQuoteDetailReport()
        {
            QuoteDetailTbl obj = new QuoteDetailTbl();
            QuotationTbl ob = new QuotationTbl();
            var custid = _unitOfWork.GetRepositoryInstance<RecordQuote>().GetAllRecords();
            var cid = custid.LastOrDefault();
            var getlast = db.QuotationTbls.ToArray().LastOrDefault();
            string ID = getlast.QuotationID.ToString();
            int QuoteID = Int32.Parse(ID);
            string CartID = cid.CartID;
            int customerid = cid.CustomerID;
            List<ReportCart> list = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CartID).ToList();
            foreach (ReportCart item in list)
            {
                QuoteDetailTbl quoteDetail = new QuoteDetailTbl()
                {
                    QuoteID = QuoteID,
                    StockID = item.StockServiceTbl.StockID,
                    Quantity = item.Quantity,

                };

                db.QuoteDetailTbls.Add(quoteDetail);
                db.SaveChanges();
            }


            return RedirectToAction("CommitQuoteDocumentReport");
        }

        public ActionResult CommitQuoteDocumentReport()
        {
            QuoteDetailTbl obj = new QuoteDetailTbl();
            QuotationTbl ob = new QuotationTbl();
            var custid = _unitOfWork.GetRepositoryInstance<RecordQuote>().GetAllRecords();
            var cid = custid.LastOrDefault();
            var getlast = db.QuotationTbls.ToArray().LastOrDefault();
            string ID = getlast.QuotationID.ToString();
            int QuoteID = Int32.Parse(ID);
            string CartID = cid.CartID;
            int customerid = cid.CustomerID;
            var custname = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerid);
            string name = custname.FirstName + " " + custname.Surname;
            string email = custname.EmailAddress;
            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CartID).ToList();
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            DataTable datatable = new DataTable();
            PdfLightTable table = new PdfLightTable();
            PdfGraphics graphics = page.Graphics;
            PdfImage image = PdfImage.FromFile(Server.MapPath("~/Images/Vees Tyre and Alignment.jpg"));
            RectangleF bounds = new RectangleF(176, 0, 390, 130);
            page.Graphics.DrawImage(image, bounds);
            PdfBrush solidbrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            graphics.DrawRectangle(solidbrush, bounds);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            PdfTextElement element = new PdfTextElement("Quotation No." + ID.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;

            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentdate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            SizeF textsize = subHeadingFont.MeasureString(currentdate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textsize.Width - 10, result.Bounds.Y);
            graphics.DrawString(currentdate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesroman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            element = new PdfTextElement("Quote To :" + name, timesroman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            PdfPen linepen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startpoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            graphics.DrawLine(linepen, startpoint, endPoint);


            datatable.Columns.Add("Item ID");
            datatable.Columns.Add("Item Name");
            datatable.Columns.Add("Item Price");
            datatable.Columns.Add("Item Quantity");
            datatable.Columns.Add("Line Total");
            int c = 0;
            foreach (QuoteCart pdfitems in list)
            {
                datatable.Rows.Add(new object[] { pdfitems.StockServiceTbl.StockID, pdfitems.StockServiceTbl.Name, pdfitems.StockServiceTbl.SellingPrice, pdfitems.Quantity, (pdfitems.Quantity * pdfitems.StockServiceTbl.SellingPrice) });
                c++;
            }

            PdfGrid pdfgrid = new PdfGrid();
            pdfgrid.DataSource = datatable;
            //pdfgrid.Draw(page, new PointF(10, 10));
            table.DataSource = datatable;
            PdfLightTableBuiltinStyleSettings settings = new PdfLightTableBuiltinStyleSettings();
            settings.ApplyStyleForBandedColumns = true;
            settings.ApplyStyleForBandedRows = true;
            settings.ApplyStyleForFirstColumn = true;
            settings.ApplyStyleForHeaderRow = true;
            settings.ApplyStyleForLastColumn = true;
            settings.ApplyStyleForLastRow = true;
            table.ApplyBuiltinStyle(PdfLightTableBuiltinStyle.ListTable6ColorfulAccent4, settings);




            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = pdfgrid.Headers[0];
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);


            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }


            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));

            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();

            layoutFormat.Layout = PdfLayoutType.Paginate;

            PdfGridLayoutResult gridResult = pdfgrid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);
            //string filepath = "C:/Users/Dylan Pather/Documents/Visual Studio 2015/Projects/GoldPrideDecoreHire/GoldPrideDecoreHire/QuotationPDF";
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            doc.Close(true);

            ms.Position = 0;
            Attachment file = new Attachment(ms, QuoteID + " Quotation.pdf", "application/pdf");


            var fromEmail = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "ikgtqxhwvonejuat";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            MailMessage message = new MailMessage(fromEmail, toEmail);
            message.Body = "Hi thank you coming today,This quote is generated from the mechanic that reported the faults.Please be advised quote is valid only for 14 days from date quoted.";
            message.Subject = "Quotation";
            message.IsBodyHtml = false;
            message.Attachments.Add(file);




            smtp.Send(message);
            //Session.Remove("cart");
            return View("QuoteSuccess");
            
        }

        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(CustomerTbl customer)
        {
            customer.PointsBalance = 0;
            customer.LoyaltyStatus = "New";
            db.CustomerTbls.Add(customer);
            db.SaveChanges();
            return RedirectToAction("AddNewCustomerVehicle");
        }

        public ActionResult AddNewCustomerVehicle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCustomerVehicle(CustomerVehicle vehicle , HttpPostedFileBase file)
        {
            var custid = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords();
            var getlast = custid.LastOrDefault();
            int customerid = getlast.CustomerID;
            vehicle.CustomerID = customerid;
            if (file != null)
            {
                vehicle.VehicleImage = new byte[file.ContentLength];
                file.InputStream.Read(vehicle.VehicleImage, 0, file.ContentLength);
            }
            db.CustomerVehicles.Add(vehicle);
            db.SaveChanges();

            return RedirectToAction("ViewMechanicReports");
        }

        public ActionResult ApproveQuote(int QuoteID)
        {
            QuotationTbl ob = new QuotationTbl();
            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.QuotationTbls.Where(yy => yy.QuotationID == QuoteID).ToList();
                foreach (var item in inv)
                {
                    item.Status = "Accepted";
                    item.DateModified = DateTime.Now;
                    dbs.SaveChanges();
                }
            }


            return RedirectToAction("Quotations");
        }
        public ActionResult DeclineQuote(int QuoteID)
        {
            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.QuotationTbls.Where(yy => yy.QuotationID == QuoteID).ToList();
                foreach (var item in inv)
                {
                    item.Status = "Declined";
                    item.DateModified = DateTime.Now;
                    dbs.SaveChanges();
                }
            }

            return RedirectToAction("Quotations");
        }

        public ActionResult BookVehicle(int QuoteID)
        {
          
            quoteid = QuoteID;
            return View();
        }

        [HttpPost]
        public ActionResult BookVehicle(BookingTbl tbl,int QuoteID)
        {
            int QuotationID = quoteid;
            
            string CartID = "";
            int VehicleID = 0;
            int bayid = 0;
            int mechanicid = 0;
            //Finds the CartID , CustomerID , VehicleID
          

            var custid = db.QuotationTbls.FirstOrDefault(a => a.QuotationID == QuoteID); 
            int customerID = custid.CustomerID;
            var cartid = db.RecordQuotes.FirstOrDefault(a => a.CustomerID == custid.CustomerID && a.VehicleID == custid.VehicleID);
            string CID = cartid.CartID;
            var report = db.ReportTbls.FirstOrDefault(a => a.CartID == CID);
            

            //DateTime dt = new DateTime(01,01,0001, 00,00,00);
            
            tbl.DateCheckedIn = DateTime.Now;
            tbl.DateCheckedOut = DateTime.Now;
            tbl.MechanicID = report.MechanicID;
            tbl.BayID = report.BayID;
            tbl.VehicleID = custid.VehicleID;
            tbl.Status = "Booked";
            tbl.QuotationID = QuoteID;
            db.BookingTbls.Add(tbl);
            

            db.SaveChanges();

            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.QuotationTbls.Where(yy => yy.QuotationID == QuoteID).ToList();
                foreach (var item in inv)
                {
                    item.Status = "Booked";
                    item.DateModified = DateTime.Now;
                    dbs.SaveChanges();
                }
            }

            return RedirectToAction("BookedVehicles");
        }

        public ActionResult BookedVehicles()
        {
            return View(_unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecords());
        }

        public ActionResult CheckIn(int bookingid)
        {
            int bayid = 0;
            using (var dbs = new ApplicationDbContext())
            {   
                var inv = dbs.BookingTbls.Where(yy => yy.BookingID == bookingid).ToList();
                foreach (var item in inv)
                {
                    item.CheckIn = true;
                    item.Status = "CheckedIn";
                    item.DateCheckedIn = DateTime.Now;
                    bayid = item.BayID;
                    dbs.SaveChanges();
                }

                
            }

            using (var dbs = new ApplicationDbContext())
            {
                var inv = dbs.WorkshopBayTbls.Where(yy => yy.BayID == bayid).ToList();
                foreach (var item in inv)
                {
                    item.IsAvailable = false;
                    dbs.SaveChanges();
                }
            }


            return RedirectToAction("BookedVehicles");
        }

        public ActionResult ViewCurrentMechanicalWork()
        {

            return View(_unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecordsIQueryable().Where(a => a.CheckIn == true && a.CheckOut == false && a.Status == "Working"));
        }


        public ActionResult ViewCompletedWork()
        {

            return View(_unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecordsIQueryable().Where(a => a.CheckIn == true && a.CheckOut == true && a.Status == "CheckedOut"));
        }


        public ActionResult Invoice(int quoteid)
        {
            Session["quoteid"] = quoteid;
            FinalInvoice ob = new FinalInvoice();
            var custid = db.QuotationTbls.FirstOrDefault(a => a.QuotationID == quoteid);
            int customerID = custid.CustomerID;
           // var cartid = db.RecordQuotes.FirstOrDefault(a => a.CustomerID == custid.CustomerID && a.VehicleID == custid.VehicleID);
            string CID = custid.CartID;
            var customer = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerID);


            var report = db.ReportTbls.FirstOrDefault(a => a.CartID == CID);
            ob.CartID = CID;
            ob.CustomerFirstName = customer.FirstName;
            ob.CustomerLastName = customer.Surname;
            ob.CustomerEmail = customer.EmailAddress;
            ob.CustomerCellNo = customer.ContactNo;

            var vehicles = db.RecordQuotes.FirstOrDefault(x => x.VehicleID == custid.VehicleID);


            var custveh = db.CustomerVehicles.FirstOrDefault(a => a.VehicleID == vehicles.VehicleID);
            ob.VehicleRegNo = custveh.VehicleRegNo;
            ob.VehicleMake = custveh.MakeOfVehicle;
            ob.VehicleModel = custveh.ModelOfVehicle;
            ob.VehicleMilleage = custveh.Mileage;
            var invoice = _unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecords();
            var id = invoice.LastOrDefault();
            ob.InvoiceID = id.InvoiceID + 1;
            decimal quotetotal = 0;

            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CID).ToList();
            
            if (list.Count() < 1)
            {
                List<ReportCart> list1 = _unitOfWork.GetRepositoryInstance<ReportCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CID).ToList();
                foreach (ReportCart item in list1)
                {
                    quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;
                    
                }
                ob.List1 = list1;
                ob.InvoiceTotalLessVat = quotetotal - (quotetotal * (decimal)0.15);
            }
            else
            {
                foreach (QuoteCart item in list)
                {
                    quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;
                    
                }
                ob.List = list;
                ob.InvoiceTotalLessVat = quotetotal - (quotetotal * (decimal)0.15);
            }

          

            ob.InvoiceTotal = quotetotal;
            ob.VAT = quotetotal * (decimal)0.15;
            ob.QuoteID = quoteid;
            Session["QID"] = quoteid;
            ob.DateOfInvoice = DateTime.Now;
            return View(ob);
        }

        public ActionResult PayNow()
        {

            return RedirectToAction("PayNow", "PayFastAdmin");
        }



        public ActionResult InvoicePost()
        {
            decimal quotetotal = 0;
            string qid = Session["QID"].ToString();
            int quoteid = Int32.Parse(qid);
            InvoiceTbl ob = new InvoiceTbl();
            var custid = db.QuotationTbls.FirstOrDefault(a => a.QuotationID == quoteid);
            int customerID = custid.CustomerID;
            var cartid = db.RecordQuotes.FirstOrDefault(a => a.CustomerID == custid.CustomerID && a.VehicleID == custid.VehicleID);
            string CID = cartid.CartID;
            var customer = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerID);

            ob.QuoteID = quoteid;
            ob.CustomerID = custid.CustomerID;

            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CID).ToList();
            

            foreach (QuoteCart item in list)
            {
                quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;

            }
            ob.AmountPaid = quotetotal;
            ob.DateOfInvoice = DateTime.Now;
            ob.PaymentType = "PayFast";

            db.InvoiceTbls.Add(ob);
            db.SaveChanges();
            return RedirectToAction("AdjustStockLevel");
        }

        public ActionResult ChangeStatusComplete()
        {
            var quoteID = _unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecords().Last();
            // string qid = Session["quoteid"].ToString();
            int quoteid = quoteID.QuoteID;
            using (var dbs = new ApplicationDbContext())
            {
                var prods = dbs.BookingTbls.Where(a => a.QuotationID == quoteid).ToList();



                foreach (var item in prods)
                {
                    item.Status = "Completed";
                    dbs.SaveChanges();
                }
            }




            return RedirectToAction("ViewInvoices");
        }
        public ActionResult AdjustStockLevel()
        {
            var quoteID = _unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecords().Last();
            string qid = Session["quoteid"].ToString();
            int quoteid = quoteID.QuoteID;
            using (var dbs = new ApplicationDbContext())
            {
                var prods = dbs.QuoteDetailTbls.Where(a => a.QuoteID == quoteid).ToList();

                foreach (var item in prods)
                {
                    item.StockServiceTbl.Quantity -= item.Quantity;
                    dbs.SaveChanges();
                }
            }

                return RedirectToAction("ChangeStatusComplete");
        }

        public ActionResult ViewInvoices()
        {
            return View(_unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecords());
        }

        public ActionResult InvoiceDetailView(int quoteid)
        {
            FinalInvoice ob = new FinalInvoice();
            var custid = db.QuotationTbls.FirstOrDefault(a => a.QuotationID == quoteid);
            int customerID = custid.CustomerID;
            var cartid = db.RecordQuotes.FirstOrDefault(a => a.CustomerID == custid.CustomerID && a.VehicleID == custid.VehicleID);
            string CID = cartid.CartID;
            var customer = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == customerID);


            var report = db.ReportTbls.FirstOrDefault(a => a.CartID == CID);
            ob.CartID = CID;
            ob.CustomerFirstName = customer.FirstName;
            ob.CustomerLastName = customer.Surname;
            ob.CustomerEmail = customer.EmailAddress;
            ob.CustomerCellNo = customer.ContactNo;

            var vehicles = db.RecordQuotes.FirstOrDefault(x => x.VehicleID == custid.VehicleID);


            var custveh = db.CustomerVehicles.FirstOrDefault(a => a.VehicleID == vehicles.VehicleID);
            ob.VehicleRegNo = custveh.VehicleRegNo;
            ob.VehicleMake = custveh.MakeOfVehicle;
            ob.VehicleModel = custveh.ModelOfVehicle;
            ob.VehicleMilleage = custveh.Mileage;
            var invoice = _unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecords();
            var id = invoice.LastOrDefault();
            ob.InvoiceID = id.InvoiceID + 1;
            decimal quotetotal = 0;

            List<QuoteCart> list = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == CID).ToList();
            ob.List = list;

            foreach (QuoteCart item in list)
            {
                quotetotal = (item.SellingPrice * item.Quantity) + quotetotal;

            }

            ob.InvoiceTotal = quotetotal;
            ob.InvoiceTotalLessVat = quotetotal - (quotetotal * (decimal)0.15);
            ob.QuoteID = quoteid;

            ob.DateOfInvoice = DateTime.Now;
            return View(ob);
        }

    }

}