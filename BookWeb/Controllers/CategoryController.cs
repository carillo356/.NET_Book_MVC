using BookWeb.Data;
using BookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = _db.Categories.ToList();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if(int.TryParse(obj.Name, out int name))
            {
                ModelState.AddModelError("name", "The Name cannot be a number");
            }

            if(ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Create Success";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Create Failed";
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if( id == null || id == 0 )
            {
                return NotFound();
            }

            Category? categoryFromDB = _db.Categories.FirstOrDefault(c => c.Id == id);
            if(categoryFromDB == null) {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (int.TryParse(obj.Name, out int name))
            {
                ModelState.AddModelError("name", "The Name cannot be a number");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Update Success";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Update Failed";
            return View();
        }

        public IActionResult Delete(int? id) {
            if(id == null || id == 0 )
            {
                return NotFound();
            }

            Category? categoryFromDB = _db.Categories.Find(id);
            
            if(categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {

            if (id == null || id == 0)
            {
                TempData["error"] = "Delete Failed";
                return NotFound();
            }

            Category? obj = _db.Categories.Find(id);
            if(obj == null)
            {
                TempData["error"] = "Delete Failed";
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Delete Success";

            return RedirectToAction("Index");
        }

    }
}
