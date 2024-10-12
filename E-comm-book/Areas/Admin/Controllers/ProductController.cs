using E_comm_book.DataAccess.Data;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
using Book.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Book.Models.ViewModel;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;

namespace E_comm_book.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitWork unitWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
          /*  var productlist = _unitofwork.Product.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitofwork.Category
                .GetAll().ToList().Select(u => new SelectListItem
                {
                    Text= u.Name,
                    Value=u.Id.ToString()
                });*/
          List<Product> productlist = _unitofwork.Product.GetAll(includeProperties:"Category").ToList();
            return View(productlist);
        }
        public IActionResult Upsert(int? id)
        {
            /* IEnumerable<SelectListItem> CategoryList =   _unitofwork.Category
                 .GetAll().ToList().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });
             /*  ViewBag.CategoryList = CategoryList; 
             ViewData["CategoryList"] = CategoryList; */
            ProductVM productVM = new ()
            {
                CategoryList = _unitofwork.Category.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                product= new Product()
            };
            if(id == null || id == 0 )
            { 
                //create
                return View(productVM); 
            }
            else
            {
                //update

                productVM.product = _unitofwork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM cat,IFormFile? file)
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
                string wwwebrootpath=_webHostEnvironment.WebRootPath;
                if(file !=null)
                {
                    string filename=Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath=Path.Combine(wwwebrootpath,@"Images\product");
                    if(!string.IsNullOrEmpty(cat.product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath=Path.Combine(wwwebrootpath,cat.product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream=new FileStream(Path.Combine(productPath,filename),FileMode.Create)) 
                    {
                        file.CopyTo(fileStream);
                    }
                    cat.product.ImageUrl= @"\Images\product\" + filename;
                }
                if (cat.product.Id == 0)
                {
                    _unitofwork.Product.Add(cat.product);

                }
                else
                {
                    _unitofwork.Product.Update(cat.product);

                }

                 _unitofwork.Save();
                TempData["success"] = "CATEGORY CREATE SUCCESSFULLY";
                return RedirectToAction("Index");
            }
            else
            {
                cat.CategoryList = _unitofwork.Category.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                    
                return View(cat);

            }
         }
        
     

        //#region API CALL

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productlist = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productlist });
        }
        [HttpDelete]
         public IActionResult Delete(int? id)
        {
            var productDelete = _unitofwork.Product.Get(u => u.Id == id);
            if(productDelete==null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var oldImagePath = Path.Combine( _webHostEnvironment.WebRootPath, productDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitofwork.Product.Remove(productDelete);
            _unitofwork.Save();
            return Json(new { success = true, message = "Deleted successfully" });

        }
        // END REGION
    }
}
