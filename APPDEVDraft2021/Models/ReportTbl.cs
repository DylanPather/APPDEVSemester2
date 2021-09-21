using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class ReportTbl
    {   [Key]
        public int ReportID { get; set; }
        public int BayID { get; set; }
        public int MechanicID { get; set; }
        public string VehicleRegNo { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public string CartID { get; set; }

    }
}