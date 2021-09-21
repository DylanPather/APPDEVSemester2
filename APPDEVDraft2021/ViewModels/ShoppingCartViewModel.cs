using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using APPDEVDraft2021.Models;

namespace APPDEVDraft2021.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }

        [DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }
    }
}