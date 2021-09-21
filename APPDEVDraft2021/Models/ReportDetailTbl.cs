using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class ReportDetailTbl
    {   [Key]
        public int ReportDetailID { get; set; }
        public int ReportID { get; set; }
        public int StockID { get; set; }
        public int Quantity { get; set; }
    }
}