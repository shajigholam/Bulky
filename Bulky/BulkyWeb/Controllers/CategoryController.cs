using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        // getting the implementation of DbContext and assign it to local var
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // retrive data from Categories table
            List<Category> objCategoryList = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
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
            Category? categoryFromDb = _db.Categories.Find(id);
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
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }
    }
}