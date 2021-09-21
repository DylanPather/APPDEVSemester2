using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class BookingTbl
    {

       [Key]
       public int BookingID { get; set; }
       public int VehicleID { get; set; }
       public int MechanicID { get; set; }
       public int BayID { get; set; }
        [Display(Name = "Date and Time Booked")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? DateBooked { get; set; }
        [Display(Name = "Date and Time Checked Out")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? DateCheckedOut { get; set; }
        public string Status { get; set; }
        public bool CheckIn { get; set; }
        public bool CheckOut { get; set; }
        [Display(Name = "Date and Time Checked In")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? DateCheckedIn { get; set; }
        public int QuotationID { get; set; }
     

    }
}