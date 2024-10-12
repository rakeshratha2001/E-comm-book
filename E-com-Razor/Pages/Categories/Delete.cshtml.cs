using E_com_Razor.Data;
using E_com_Razor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_com_Razor.Pages.Categories
{

    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                category = _db.Categories.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            
                Category? cat = _db.Categories.Find(category.Id);
                    if(cat==null)
                {
                    return NotFound();
                }
                _db.Categories.Remove(cat);
                _db.SaveChanges();
            TempData["success"] = "CATEGORY DELETE SUCCESSFULLY";
            //TempData["success"] = "CATEGORY EDIT SUCCESSFULLY";

            return RedirectToPage("Index");
         
             
        }
    }
}
