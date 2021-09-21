using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class WorkshopBayTbl
    {
     [Key]
     public int BayID { get; set; }
     public string BayType { get; set; }
     public bool IsAvailable { get; set; }
    }
}