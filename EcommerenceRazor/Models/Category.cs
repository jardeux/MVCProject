using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EcommerenceRazor.Models
{
    public class Category
    {

        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]//koşul ekledik
        public string Name { get; set; }
        [DisplayName("Display Order")]

        [Range(0, 100)] //koşul ekledik
        public int DisplayOrder { get; set; }

    }
}
