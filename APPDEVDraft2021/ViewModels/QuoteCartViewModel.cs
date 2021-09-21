using APPDEVDraft2021.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.ViewModels
{
    public class QuoteCartViewModel
    {
        public List<QuoteCart> CartItems { get; set; }

        [DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }
    }
}