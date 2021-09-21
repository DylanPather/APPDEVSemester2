using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class QuoteCart
    {

       
            [Key]
            public int QuoteCartID { get; set; }
            public string CartID { get; set; }
            public int StockID { get; set; }
            public int Quantity { get; set; }
            public decimal SellingPrice { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public virtual StockServiceTbl StockServiceTbl { get; set; }
        
    }
}