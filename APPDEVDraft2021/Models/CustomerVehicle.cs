using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class CustomerVehicle
    {
        [Key]
        public int VehicleID { get; set; }
        public string VehicleRegNo { get; set; }
        public int CustomerID { get; set; }
        public string MakeOfVehicle { get; set; }
        public int ModelOfVehicle { get; set; }
        public int Mileage { get; set; }
        public byte[] VehicleImage { get; set; }
        public CustomerTbl CustomerTbl { get; set; }
    }
}