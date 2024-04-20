using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        // injecting IUnitOfWork, which acts as a higher-level abstraction that encapsulates one or more repositories.
        private readonly IUnitOfWork _unitOfWork;
        // asking dependancy injection to provide the impl
        public CategoryController(IUnitOfWork unitOfWork)
        {
            // getting the implementation of IUnitOfWork and assign it to local var
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // retrive data from Category repository
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        // create a get action method
        public IActionResult Create()
        {
            // by default passing a new Category obj
            return View();
        }

        // when hit submit button
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // custom validation
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    // key is name, so the custom error is shown for this field
            //    ModelState.AddModelError("name", "The dispaly order cannot exactly match the name.");
            //}
            if (ModelState.IsValid)
            {
                // add a new Category obj
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully!";
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        // when hit submit button
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                // update the Category obj
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully!";
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        // we cannot use the same delete name as we have the same name and parameter for the prev method. instead we give it an action name to be found correctly
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            // delete the Category obj
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}