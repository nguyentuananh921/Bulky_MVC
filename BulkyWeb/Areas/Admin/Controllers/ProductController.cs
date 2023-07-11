using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.Models;
using BulkyBook.DataAccess.Data.ApplicationDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BulkyBook.Models.Models;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {        
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }             

        // GET: Product
        //public async Task<IActionResult> Index()
        public IActionResult Index()
        {            
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
            return View(objProductList);
        }

        // GET: Product/Details/5
        //public async Task<IActionResult> Details(int? id)
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _db.Categories
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var product = _unitOfWork.ProductRepository.Get(u=>u.Id==id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Product category)
        //public async Task<IActionResult> Create(Product category)
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)  //It will check validation in the Product Model
            {
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();  //Save to database

                TempData["success"] = "Product Created successfully";//TempData with the keyname of success
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Product"); //In the same controller no need to specify controller Name
            }
            return View(product);
        }

        // GET: Product/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        public IActionResult Edit(int? id)
        {
            if (id == null||id==0)
            {
                return NotFound();
            }            
            var productFromDb = _unitOfWork.ProductRepository.Get(u=>u.Id==id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DisplayOrder")] Product category)
        //public async Task<IActionResult> Edit(Product category)
        //public async Task<IActionResult> Edit(int id, Product category)
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }           

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";//TempData with the keyname of success                
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _db.Categories
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var category = _unitOfWork.ProductRepository.Get(u=>u.Id==id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Product/Delete/5
        //Can't set the same name for the GET and POST method due to parameter is the same.
        [HttpPost, ActionName("Delete")] //Explicit The name of Endpoint
        [ValidateAntiForgeryToken]
        
        public IActionResult DeleteConfirmed(int id) 
            
        {
            //var product = await _db.Categories.FindAsync(id);

            var product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product != null)
            {
                //_db.Categories.Remove(category);
                _unitOfWork.ProductRepository.Remove(product); 
                _unitOfWork.Save();
            }

            //await _db.SaveChangesAsync();
            TempData["success"] = "Product deleted successfully";//TempData with the keyname of success
            return RedirectToAction(nameof(Index));
        }
        
    }
}
