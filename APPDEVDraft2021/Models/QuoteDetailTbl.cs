using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class QuoteDetailTbl
    {
        [Key]
        public int QuoteDetailID { get; set; }
        public int QuoteID { get; set; }
        public int StockID { get; set; }
        public int Quantity { get; set; }
        
        public virtual StockServiceTbl StockServiceTbl { get; set; }
        public QuotationTbl QuotationTbl { get; set; }
    }
}