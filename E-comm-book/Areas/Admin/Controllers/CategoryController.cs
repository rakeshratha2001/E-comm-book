using E_comm_book.DataAccess.Data;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
using Book.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;

namespace E_comm_book.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitWork _unitofwork;

        public CategoryController(IUnitWork unitWork)
        {
            _unitofwork = unitWork;
        }

        public IActionResult Index()
        {
            var categorylist = _unitofwork.Category.GetAll().ToList();
            return View(categorylist);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category cat)
        {
            if (cat.Name == cat.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "the display order can not match the namme");
            }
            /*if (cat.Name!=null && cat.Name.ToLower() == "text")
            {
                ModelState.AddModelError("","text is not valid");
            }*/
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(cat);
                _unitofwork.Save();
                TempData["success"] = "CATEGORY CREATE SUCCESSFULLY";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitofwork.Category.Get(u => u.Id == id);
            /* Category? category1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
             Category? category2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();*/

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category cat)
        {
            /*  if (cat.Name == cat.DisplayOrder.ToString())
              {
                  ModelState.AddModelError("name", "the display order can not match the namme");
              }
              /*if (cat.Name!=null && cat.Name.ToLower() == "text")
              {
                  ModelState.AddModelError("","text is not valid");
              }*/
            if (ModelState.IsValid)
            {

                _unitofwork.Category.Update(cat);
                _unitofwork.Save();
                TempData["success"] = "CATEGORY EDIT SUCCESSFULLY";

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitofwork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? cat = _unitofwork.Category.Get(u => u.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            _unitofwork.Category.Remove(cat);
            _unitofwork.Save();
            TempData["success"] = "CATEGORY DELETE SUCCESSFULLY";

            return RedirectToAction("Index");

        }
    }
}
