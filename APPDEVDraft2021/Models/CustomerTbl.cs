using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class CustomerTbl
    {   [Key]
        public int CustomerID { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ContactNo { get; set; }
        public int PointsBalance { get; set; }
        public string LoyaltyStatus { get; set; }

        public ICollection<CustomerTbl> Customer { get; set; }
    }

  
}