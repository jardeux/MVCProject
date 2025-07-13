using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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





    }
}
