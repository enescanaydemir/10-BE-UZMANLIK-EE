using System;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.ResponseDtos;

namespace EShop.Services.Abstract;

public interface ICartService
{
    Task<ResponseDto<CartDto>> GetAsync(int id); // Id'ye göre sepet getir
    Task<ResponseDto<IEnumerable<CartDto>>> GetAllAsync(); // Tüm sepetleri getir
    Task<ResponseDto<IEnumerable<CartDto>>> GetAllAsync(bool? isActive); // Aktif olan sepetleri getir
    Task<ResponseDto<IEnumerable<CartDto>>> GetByUserIdAsync(int userId); // Kullanıcıya ait sepetleri getir!!!!
    Task<ResponseDto<CartDto>> AddAsync(CartCreateDto cartCreateDto); // Sepet ekleme
    Task<ResponseDto<NoContent>> UpdateAsync(CartUpdateDto cartUpdateDto); // Sepet güncelleme. Normalde burda ek olarak Update imzası için bool tipinde aktifliğini kontrol etmek istedim. ancak sonradan şunu fark ettim; zaten CartUpdateDto içerisinde IsActive ve IsDeleted alanları var. Bu yüzden burada ekstra bir kontrol yapmama gerek yok.
    Task<ResponseDto<NoContent>> SoftDeleteAsync(int id); // Sepeti silme
    Task<ResponseDto<NoContent>> HardDeleteAsync(int id); // Sepeti veritabanından silme



    //Sepet işlemleri için klasik dışı eklediğimm metotd imzalari. Ancak bunların dto larını farklı tanımlamışız. o zaman aşağıdaki metot imzalarını cartItem olarak farklı bir interface de mi tanımlamamız gerekecek ? 
    //Not: Bu soruyu araştırırken aggregation yani birlikte kullanılan ilişkiyi gördüm. Bu yüzden bu metotları ayrı bir interface de tanımlamama gerek yok sanırım. Çünkü cartItem işlemleri cart işlemleri ile birlikte kullanılıyor(CartDto içinde Collection olarak tanımlı). bir nevi zorunluluk.
    Task<ResponseDto<NoContent>> AddCartItemAsync(CartItemCreateDto cartItemCreateDto); // Sepete ürün ekleme
    Task<ResponseDto<NoContent>> UpdateCartItemAsync(CartItemUpdateDto cartItemUpdateDto); // Sepetteki ürünü güncelleme
    Task<ResponseDto<NoContent>> UpdateCartItemQuantityAsync(CartItemUpdateDto cartItemUpdateDto); // Sepetteki ürünün adetini güncelleme işlemi. burda parametre olarak id ve quantity alıyoruz.çünkü id ile ürünü bulup quantity ile güncelleme yapılacaak
    Task<ResponseDto<NoContent>> DeleteCartItemAsync(CartItemDto cartItemDto); // Sepetten ürün silme
    Task<ResponseDto<NoContent>> DeleteAllCartItemAsync(int id); // Sepetteki tüm ürünleri silme
    Task<ResponseDto<NoContent>> ClearCartAsync(int id); // Sepetteki tümm ürünleri islme  

}
