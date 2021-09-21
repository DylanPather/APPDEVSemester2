using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public partial class CustVehCheck
    {
       public int CustomerID { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string CellNo { get; set; }
        public int PointsBalance { get; set; }
        public string LoyaltyStatus { get; set; }
    }
}