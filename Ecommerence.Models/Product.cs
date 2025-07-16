using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerence.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Product Price")]
        [Range(1, 1000.00)]
        public double ListPrice { get; set; }
        [Required]
        [Display(Name = "50+ Product Price")]
        [Range(1, 1000.00)]
        public double Price50 { get; set; }
        [Required]
        [Display(Name = "100+ Product Price")]
        [Range(1, 1000.00)]
        public double Price100 { get; set; }
        [Required]
        [Display(Name = "0-50 Product Price")]
        [Range(1, 1000.00)]
        public double Price { get; set; }
        public int CategoryIdFK { get; set; }
        
        [ForeignKey("CategoryIdFK")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }




    }
}
