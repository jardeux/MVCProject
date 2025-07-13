using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace e_commerenceMVC.Models
{
    public class Category
    {
        
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]//koşul ekledik
        public string Name { get; set; }
        [DisplayName("Display Order")]

        [Range(0,100)] //koşul ekledik
        public int DisplayOrder { get; set; }

    }
}
