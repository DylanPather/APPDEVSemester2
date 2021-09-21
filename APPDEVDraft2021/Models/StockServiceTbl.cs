using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVDraft2021.Models
{
    public class StockServiceTbl
    {   [Key]
        public int StockID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] ProductImage { get; set; }
        public bool IsFeatured { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }

        public CategoryTbl CategoryTbl { get; set; }
    }
}