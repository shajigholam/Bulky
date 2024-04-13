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

        // create an action method
        public IActionResult Create()
        {
            // by default passing a new Category obj
            return View();
        }

        // when hit submit button
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            return View();
        }
    }
}