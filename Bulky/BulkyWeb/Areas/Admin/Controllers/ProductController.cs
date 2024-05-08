using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // injecting IUnitOfWork, which acts as a higher-level abstraction that encapsulates one or more repositories.
        private readonly IUnitOfWork _unitOfWork;
        // to access wwwroot
        private readonly IWebHostEnvironment _webHostEnvironment;
        // asking dependancy injection to provide the impl
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            // getting the implementation of IUnitOfWork and assign it to local var
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // retrive data from Product repository
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }

        // create a get action method
        public IActionResult Upsert(int? id)
        {
            // projection in EF Core -> dynamic conversion when retrieving data from db(for example here we're selecting 2 colomns from the category & convert it to add it as a new obj with type )
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});

            //ViewBag
            //ViewBag.CategoryList = CategoryList;
            //ViewData
            //ViewData["CategoryList"]=CategoryList;

            ProductVM productVM = new()
            { 
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                // create
                return View(productVM);
            }
            else
            {
                // update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        // when hit submit button
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // delete the old one
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //save the img
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    // add a new Product obj
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                // to make sure if we run into any errors, we populate the drop down again
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        // we cannot use the same delete name as we have the same name and parameter for the prev method. instead we give it an action name to be found correctly
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            // delete the Product obj
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }
        #endregion
    }
}
/*
 * ViewBag
 * ViewBag transfers data from the controller to view, not vice-versa. Ideal for situations in which the temporary data is not a model.
 * ViewBag is a dynamic property. any number of properties and values can be assigned to viewbag.
 * the ViewBag's life lasts during the current http request. ViewBag will be null if redirection occurs.
 * ViewBag is actually a wrapper around ViewData.
 * ViewData
 * ViewData is derived from ViewDataDictionary which is a dictionary type.
 * ViewData value must be type cast before use.
 * ViewData's life lasts during the current http request. ViewBag will be null if redirection occurs.
 * 
 * ViewBag internally inserts data into viewData dictionary. so the key of ViewData and property of ViewBag must NOT match.
 * TempData
 * TempData can be used to store data between between two consecutive requests.
 * TempData internally use session to store the data. so think of it as a short lived session.
 * TempData value must be type cast before use. check for null values to avoid run time error.
 * TempData can be used to store only one time messages like error or validation messages.
 */

//public IActionResult Edit(int? id)
//{
//    if (id == null || id == 0)
//    {
//        return NotFound();
//    }
//    Product? categoryFromDb = _unitOfWork.Product.Get(u => u.Id == id);

//    if (categoryFromDb == null)
//    {
//        return NotFound();
//    }

//    return View(categoryFromDb);
//}

//// when hit submit button
//[HttpPost]
//public IActionResult Edit(Product obj)
//{
//    if (ModelState.IsValid)
//    {
//        // update the Product obj
//        _unitOfWork.Product.Update(obj);
//        _unitOfWork.Save();
//        TempData["success"] = "Product updated successfully!";
//        return RedirectToAction("Index");
//    }
//    return View();

//}
