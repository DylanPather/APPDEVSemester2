using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public partial class MechanicDashboardCard
    {
        public int NoOfBookedVehicles { get; set; }
        public int NoOfCompletedVehicles { get; set; }
        public int NoWorkBaysAvailable { get; set; }
        public int ReportsMade { get; set; }
    }
}