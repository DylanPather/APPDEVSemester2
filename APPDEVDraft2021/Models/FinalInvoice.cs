using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public partial class FinalInvoice
    {
        public decimal InvoiceTotal { get; set; }


        public DateTime DateOfInvoice { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerCellNo { get; set; }
        public string CustomerEmail { get; set; }
        public string VehicleRegNo { get; set; }
        public string VehicleMake { get; set; }
        public int VehicleModel { get; set; }
        public int VehicleMilleage { get; set; }
        public decimal VAT { get; set; }
        public decimal InvoiceTotalLessVat { get; set; }
        public List<QuoteCart> List { get; set; }
        public List<ReportCart> List1 { get; set; }
        public int InvoiceID { get; set; }
        public int QuoteID { get; set; }
        public string CartID { get; set; }
        public virtual StockServiceTbl stockservice { get; set; }
        public virtual QuoteCart quotecart { get; set; }
    }
}