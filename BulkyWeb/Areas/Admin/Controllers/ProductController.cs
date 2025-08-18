using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? files)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                }

                _unitOfWork.Save();


                //string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {

                    //    foreach (IFormFile file in files)
                    //    {
                    //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //        string productPath = @"images\products\product-" + productVM.Product.Id;
                    //        string finalPath = Path.Combine(wwwRootPath, productPath);

                    //        if (!Directory.Exists(finalPath))
                    //            Directory.CreateDirectory(finalPath);

                    //        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    //        {
                    //            file.CopyTo(fileStream);
                    //        }

                    //        ProductImage productImage = new()
                    //        {
                    //            ImageUrl = @"\" + productPath + @"\" + fileName,
                    //            ProductId = productVM.Product.Id,
                    //        };

                    //        if (productVM.Product.ProductImages == null)
                    //            productVM.Product.ProductImages = new List<ProductImage>();

                    //        productVM.Product.ProductImages.Add(productImage);

                    //    }

                    _unitOfWork.ProductRepository.Update(productVM.Product);
                    _unitOfWork.Save();




                }


                    TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
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
            Product? productFromDb = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Remove(product);
                _unitOfWork.Save();
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
