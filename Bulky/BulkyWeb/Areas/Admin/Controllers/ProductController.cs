using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // injecting IUnitOfWork, which acts as a higher-level abstraction that encapsulates one or more repositories.
        private readonly IUnitOfWork _unitOfWork;
        // asking dependancy injection to provide the impl
        public ProductController(IUnitOfWork unitOfWork)
        {
            // getting the implementation of IUnitOfWork and assign it to local var
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // retrive data from Product repository
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            // projection in EF Core -> dynamic conversion when retrieving data from db(for example here we're selecting 2 colomns from the category & convert it to add it as a new obj with type )
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u=>new SelectListItem
            {
                Text = u.Name,
                Value=u.Id.ToString()
            });
            return View(objProductList);
        }

        // create a get action method
        public IActionResult Create()
        {
            // by default passing a new Product obj
            return View();
        }

        // when hit submit button
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                // add a new Product obj
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
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
            Product? categoryFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        // when hit submit button
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                // update the Product obj
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully!";
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
    }
}