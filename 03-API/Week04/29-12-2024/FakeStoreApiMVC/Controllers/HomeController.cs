using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FakeStoreApiMVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace FakeStoreApiMVC.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FakeStoreApi"); //HttpClient sınıfını FakeStoreApi isimli servisten alıyoruz.
    }

    public async Task<IActionResult> Index() //Anasayfa. Tüm ürünleri listeler.
    {
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("products"); //API'ye GET isteği gönderiyoruz
        string contentResponse = await responseMessage.Content.ReadAsStringAsync(); //API'den gelen veriyi okuyoruz
        List<Product>? response = JsonConvert.DeserializeObject<List<Product>>(contentResponse); //API'den gelen veriyi Product Listesine çeviriyoruz.


        return View(response);
    }

    public async Task<IActionResult> Details(int id) //Ürün detay sayfası. id parametresi ile ürün id'sini alıyoruz.
    {
        var responseMessage = await _httpClient.GetAsync($"products/{id}"); //API'ye GET isteği gönderiyoruz
        var contentResponse = await responseMessage.Content.ReadAsStringAsync(); //API'den gelen veriyi okuyoruz
        var response = JsonConvert.DeserializeObject<Product>(contentResponse); //API'den gelen veriyi Product sınıfına çeviriyoruz.
        return View(response);
    }

    public async Task<IActionResult> GetCategories()
    {
        var responseMessage = await _httpClient.GetAsync("products/categories");
        var contentResponse = await responseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<List<string>>(contentResponse);
        return View(response);
    }

    public async Task<IActionResult> AddProduct()
    {
        var responseMessage = await _httpClient.GetAsync("products/categories");
        var contentResponse = await responseMessage.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<List<string>>(contentResponse);
        ViewBag.Categories = categories; //View'a göndermek için ViewBag kullanıyoruz. ViewBag, Controller'dan View'a veri taşımak için kullanılır.
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            // var response = await _httpClient.PostAsJsonAsync("products", product);

            var serializeProduct = JsonConvert.SerializeObject(product);
            HttpContent content = new StringContent(serializeProduct, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("products", content);
            var newProduct = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(newProduct.Result);
            return Json(result);
        }
        var responseMessage = await _httpClient.GetAsync("products/categories");
        var contentResponse = await responseMessage.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<List<string>>(contentResponse);
        ViewBag.Categories = categories;
        return View(product);
    }
}
