using EShop.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EShop.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;

        public ProductController(IProductService productService, ICategoryService categoryService, IToastNotification toastNotification)
        {
            _productService = productService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllAsync();
            return View(response.Data);//Bilerek hata kontrolü yapmadık, aslında doğru olan yapmak.
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIsActive(int id)
        {
            var response = await _productService.UpdateIsActiveAsync(id);
            Console.WriteLine(response.Data);
            Console.WriteLine(response.Error);
            Console.WriteLine(response.IsSuccessful);
            return Json(new { isSuccessful = response.IsSuccessful, error = response.Error });
        }

        [HttpDelete]
        public async Task<IActionResult> HardDelete(int id)
        {
            var response = await _productService.HardDeleteAsync(id);
            return Json(new { isSuccessful = response.IsSuccessful, error = response.Error });
        }

        [HttpDelete]
        public async Task<IActionResult> SoftDelete(int id)
        {
            Console.WriteLine("Silinecek Ürün Id: " + id);
            var response = await _productService.SoftDeleteAsync(id);
            return Json(new { isSuccessful = response.IsSuccessful, error = response.Error });
        }

    }
}
