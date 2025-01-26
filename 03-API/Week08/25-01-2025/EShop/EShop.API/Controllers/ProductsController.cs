using EShop.Services.Abstract;
using EShop.Shared.ControllerBases;
using EShop.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : CustomControllerBase
    {
        private readonly IProductService _productManager;

        public ProductsController(IProductService productManager)
        {
            _productManager = productManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductCreateDto productCreateDto)
        {
            var response = await _productManager.AddAsync(productCreateDto);
            return CreateResult(response);
        }
    }
}

//servis tarafında yazdığımız kodlar burayı daha yalın tutmamızı sağladı.