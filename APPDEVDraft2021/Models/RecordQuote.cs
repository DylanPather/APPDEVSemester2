using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class RecordQuote
    {   [Key]
        public int RQuoteID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public string CartID { get; set; }



    }
}