using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        //public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
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
            var product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Upsert
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepository
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
                return View(productVM);

            }


        }

        // POST: Product/Upsert
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Product category)
        //public async Task<IActionResult> Create(Product category)
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            //int CheckCategoryId = productVM.Product.CategoryId;
            if (ModelState.IsValid)  //It will check validation in the Product Model
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //Delete old image
                        if (productVM.Product.ImageUrl != null) //Make sure old ImageUrl not null 
                        {
                            var oldImagePath =
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);//Add new Product
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);//Update product
                }
                _unitOfWork.Save();  //Save to database

                TempData["success"] = "Product Created successfully";//TempData with the keyname of success
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Product"); //In the same controller no need to specify controller Name
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryRepository
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                return View(productVM);
            }

        }

        // GET: Product/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var category = await _db.Categories
        //    //    .FirstOrDefaultAsync(m => m.Id == id);
        //    var productToDelete = _unitOfWork.ProductRepository.Get(u => u.Id == id);
        //    if (productToDelete == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productToDelete);
        //}

        // POST: Product/Delete/5
        //Can't set the same name for the GET and POST method due to parameter is the same.
        //[HttpPost, ActionName("Delete")] //Explicit The name of Endpoint
        //[ValidateAntiForgeryToken]

        //public IActionResult DeleteConfirmed(int id)

        //{
        //    //var product = await _db.Categories.FindAsync(id);

        //    var product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
        //    if (product != null)
        //    {
        //        //_db.Categories.Remove(category);
        //        _unitOfWork.ProductRepository.Remove(product);
        //        _unitOfWork.Save();
        //    }

        //    //await _db.SaveChangesAsync();
        //    TempData["success"] = "Product deleted successfully";//TempData with the keyname of success
        //    return RedirectToAction(nameof(Index));
        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
            //return Json(objProductList);

        }        
        //[HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.ProductRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
