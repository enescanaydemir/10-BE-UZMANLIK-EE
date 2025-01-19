using EShop.Service.Abstract;
using EShop.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    // uygulamaadresi/api/auths şeklinde bir istek geldiğinde bu controller çalışır.
    // [Route("api/[controller]")] 
    // Bu şekilde bir rota belirlediğimizde, bu adrese;
    // POST metodu ile bir istek geldiğinde HttpPost tipindeki metot(Login) çalışacak.
    // GET metodu ile bir istek geldiğinde HttpGet tipindeki metot(???) çalışacak.
    // PUT metodu ile bir istek geldiğinde HttpPut tipindeki metot(???) çalışacak.
    // DELETE metodu ile bir istek geldiğinde HttpDelete tipindeki metot(???) çalışacak.
    [Route("api/auths")] //restful api yapısında controller isimlerinin sonuna Controller kelimesi eklenmez. Büyük harfle başalamsın kontrolü gibi bir kontrolle ilgili. Bu yüzden [controller] yerine controller ismi yazılır.
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto); //burasının tipi nasıl TokenDto oldu ? 
            return StatusCode(result.StatusCode, result); //burda return olan result, json formatında dönüyor. Dönen response body de statusCoden gereksiz yere var çünkü http protokolü zaten result ile verdiğimiz statusCode u dönüyor. gereksiz yere http protokolü dışında api tarafında da statusCode u döndürmemek için ResponseDto.cs içinde yani tanımlandığı yerde [JsonIgnore] attribute u ekledik.
        }

        [HttpPost("register")] //api/auths/register şeklinde bir istek geldiğinde bu metot çalışır. ek olarak yanına register yazmamızın sebebği api url ini ayırt edip özelleştirmek için yapılır.
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
