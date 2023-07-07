using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Models;
using Bulky.DataAccess.Data.ApplicationDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bulky.Models.Models;


namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            List<Category> objCategoryList = await _db.Categories.ToListAsync();
            //return View();
            return View(objCategoryList);            
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Category category)
        public async Task<IActionResult> Create(Category category)
        {
            //if (category.Name == category.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name","The DisplayOrder can't exactly match the Name.");
            //}

            //if (category.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "Test is an invalid value");
            //}
            if (ModelState.IsValid)  //It will check validation in the Category Model
            {
                _db.Add(category);
                await _db.SaveChangesAsync();  //Save to database

                TempData["success"] = "Category Created successfully";//TempData with the keyname of success
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index","Category"); //In the same controller no need to specify controller Name
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null||id==0)
            {
                return NotFound();
            }

            //var categoryFromDb = await _db.Categories.FindAsync(id);
            Category? categoryFromDb = _db.Categories.Find(id);//Only find with Id
            Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);//Can find by any field
            Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DisplayOrder")] Category category)
        //public async Task<IActionResult> Edit(Category category)
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(category);  //If Id is not exist it will create a new category
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Category updated successfully";//TempData with the keyname of success
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        //Can't set the same name for the GET and POST method due to parameter is the same.
        [HttpPost, ActionName("Delete")] //Explicit The name of Endpoint
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(int id) 
            
        {
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
            }

            await _db.SaveChangesAsync();
            TempData["success"] = "Category deleted successfully";//TempData with the keyname of success
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _db.Categories.Any(e => e.Id == id);
        }
    }
}
