using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using APPDEVDraft2021.Models;

namespace APPDEVDraft2021.ViewModels
{
    public class ReportCartViewModel
    {
        public List<ReportCart> CartItems { get; set; }

        [DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }
    }
}