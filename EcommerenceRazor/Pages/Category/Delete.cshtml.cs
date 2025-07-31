using EcommerenceRazor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerenceRazor.Pages.Category
{

    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public EcommerenceRazor.Models.Category? CategoryList { get; set; } // Nullable olarak tanýmla

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet(int? id)
        {
            CategoryList = _db.Categories.Find(id);
        }
        public IActionResult OnPost()
        {
            if (CategoryList == null)
            {
                return NotFound();
            }
            var obj = _db.Categories.Find(CategoryList.CategoryId);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            TempData["success"] = "Kategori baþarýyla silindi.";
            _db.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
