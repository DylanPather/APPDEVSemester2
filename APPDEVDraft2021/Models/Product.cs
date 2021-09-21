using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APPDEVDraft2021.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Add Name of product")]
        public string PName { get; set; }

        [DisplayName("Type of Product")]
        [Required(ErrorMessage = "select product Type")]
        public string ProdType { get; set; }

        [DisplayName("Stock")]
        [Required(ErrorMessage = "enter amount of stock available")]
        [Range(1, 1000)]
        public int Stock { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Enter description of product")]
        public string Description { get; set; }

        [DisplayName("price")]
        [Required(ErrorMessage = "enter product pricing")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DisplayName("Product Image")]

        public string ImageLocation { get; set; }

       
    }
}