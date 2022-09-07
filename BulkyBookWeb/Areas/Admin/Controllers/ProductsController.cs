using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = db;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        //GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

        //    if (productFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDbFirst);
        //}

        // GET - Combine Create and Update
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                ,
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };

            if (id == null || id == 0)
            {
                // create
                return View(productVM);
            }
            else
            {
                // update
                return View();
            }
        }


        //POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string uploads = Path.Combine(_hostEnvironment.WebRootPath, @"images\products\");
                    //string extension = Path.GetExtension(file.FileName);
                    string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"images\products\" + fileName;
                }

                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (productFromDbFirst == null)
            {
                return NotFound();
            }

            return View(productFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        #region  API CALLS
        // admin/product/getall
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll();
            return Json(new { data = productList });
        }

        #endregion
    }
}
