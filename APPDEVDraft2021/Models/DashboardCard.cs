using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public partial class DashboardCard
    {

        public int StockCount { get; set; }
        public int QuotationCount { get; set; }
        public int InvoiceCount { get; set; }
        public int UnassignedBays { get; set; }
       
        public int ContactUsCount { get; set; }
        public int AppointmentBooking { get; set; }
    }
}