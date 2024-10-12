using E_com_Razor.Data;
using E_com_Razor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_com_Razor.Pages.Categories
{
    [BindProperties]
    public class createModel : PageModel
    {
         
        private readonly ApplicationDbContext _db;
         public  Category  category { get; set; }
        public createModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            TempData["success"] = "CATEGORY CREATE SUCCESSFULLY";
            return RedirectToPage("Index");
        }
    }
}
