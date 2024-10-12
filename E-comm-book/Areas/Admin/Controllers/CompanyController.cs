using Book;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
using E_comm_book.DataAccess;
using System.Collections.Generic;
using Book.Models.ViewModel;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Book.DataAccess.Repository.IRepository;

namespace E_comm_book.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitWork _unitofwork;
         public CompanyController(IUnitWork unitWork )
        {
            _unitofwork = unitWork;
         }
        public IActionResult Index()
        {
          /*  var Companylist = _unitofwork.Company.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitofwork.Category
                .GetAll().ToList().Select(u => new SelectListItem
                {
                    Text= u.Name,
                    Value=u.Id.ToString()
                });*/
          List<Company> Companylist = _unitofwork.Company.GetAll().ToList();
            return View(Companylist);
        }
        public IActionResult Upsert(int? id)
        {
            /* IEnumerable<SelectListItem> CategoryList =   _unitofwork.Category
                 .GetAll().ToList().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });
               ViewBag.CategoryList = CategoryList; 
             ViewData["CategoryList"] = CategoryList; 
            Company CompanyVM = new ()
            {
                CategoryList = _unitofwork.Category.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Company= new Company()
            }; */
            if (id == null || id == 0 )
            { 
                //create
                return View(new Company()); 
            }
            else
            {
                //update

                Company companyobj = _unitofwork.Company.Get(u => u.Id == id);
                return View(companyobj);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(Company companyobj)
        {
            /* if (cat.Name == cat.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "the display order can not match the namme");
            }
            if (cat.Name!=null && cat.Name.ToLower() == "text")
            {
                ModelState.AddModelError("","text is not valid");
            }*/
            if (ModelState.IsValid)
            {
               /* string wwwebrootpath=_webHostEnvironment.WebRootPath;
                if(file !=null)
                {
                    string filename=Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string CompanyPath=Path.Combine(wwwebrootpath,@"Images\Company");
                    if(!string.IsNullOrEmpty(cat.Company.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath=Path.Combine(wwwebrootpath,cat.Company.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream=new FileStream(Path.Combine(CompanyPath,filename),FileMode.Create)) 
                    {
                        file.CopyTo(fileStream);
                    }
                    cat.Company.ImageUrl= @"\Images\Company\" + filename;
                } */
                if ( companyobj.Id == 0)
                {
                    _unitofwork.Company.Add( companyobj);

                }
                else
                {
                    _unitofwork.Company.Update(companyobj);

                }

                 _unitofwork.Save();
                TempData["success"] = "CATEGORY CREATE SUCCESSFULLY";
                return RedirectToAction("Index");
            }
            else
            {
              /*  cat.CategoryList = _unitofwork.Category.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }); */
                    
                return View(companyobj);

            }
         }
        
     

        //#region API CALL

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> Companylist = _unitofwork.Company.GetAll().ToList();
            return Json(new { data = Companylist });
        }
        [HttpDelete]
         public IActionResult Delete(int? id)
        {
            var CompanyDelete = _unitofwork.Company.Get(u => u.Id == id);
            if(CompanyDelete==null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            

            _unitofwork.Company.Remove(CompanyDelete);
            _unitofwork.Save();
            return Json(new { success = true, message = "Deleted successfully" });

        }
        // END REGION
    }
}
