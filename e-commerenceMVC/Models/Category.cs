using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace e_commerenceMVC.Models
{
    public class Category
    {
        
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]

        public int DisplayOrder { get; set; }

    }
}
