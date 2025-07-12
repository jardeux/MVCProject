using EcommerenceRazor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerenceRazor.Pages.Category
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public EcommerenceRazor.Models.Category CategoryList { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            if(id!=null && id!=0)
            {
                CategoryList = _db.Categories.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(CategoryList);
                //TempData["success"] = "Kategori baþarýyla eklendi.";
                _db.SaveChanges();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}

