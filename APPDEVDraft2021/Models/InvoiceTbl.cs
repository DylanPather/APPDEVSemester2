using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class InvoiceTbl
    {
        [Key]
        public int InvoiceID { get; set; }
        public int QuoteID { get; set; }
        public int CustomerID { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DateOfInvoice { get; set; }
        public string PaymentType { get; set; }

    }
}