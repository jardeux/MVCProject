using EcommerenceRazor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerenceRazor.Models;

namespace EcommerenceRazor.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<EcommerenceRazor.Models.Category> CategoryList { get; set; } // Fully qualify the 'Category' type

        public IndexModel(ApplicationDbContext db)
        {
            _db = db; // Fix assignment order
        }

        public void OnGet()
        {
            CategoryList = _db.Categories.ToList(); // Kategorileri veritabanýndan getir
        }
    }
}
