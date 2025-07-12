using EcommerenceRazor.Data;
using EcommerenceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerenceRazor.Pages.Category
{   

    // To fix the CS0592 error, move [BindProperties] above the class declaration:

    [BindProperties] // Correct usage: applies to the class
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EcommerenceRazor.Models.Category CategoryList { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(CategoryList);
                TempData["success"] = "Kategori baþarýyla eklendi.";
                _db.SaveChanges();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
