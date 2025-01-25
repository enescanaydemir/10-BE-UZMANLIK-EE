using System;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.ResponseDtos;

namespace EShop.Services.Abstract;

public interface ICartService
{
    Task<ResponseDto<CartDto>> CreateCartAsync(string applicationUserId); // Id'ye göre sepet oluşturma. CartId ye göre değil UserId ye göre sepet oluşturulacak çünkü login olan kişinin id si elimizde olduğu için UserId ye göre sepet oluşturacağız. kişinin id sine göre sepet oluşturulacak
    Task<ResponseDto<CartDto>> GetCartAsync(string applicationUserId); // Id'ye göre sepet getir. CartId ye göre değil UserId ye göre sepet getirilecek çünkü login olan kişinin id si elimizde olduğu için UserId ye göre sepeti getireceğiz. kişinin id sine göre sepet getirilecek


    //Sepet işlemleri için klasik dışı eklediğimm metotd imzalari. Ancak bunların dto larını farklı tanımlamışız. o zaman aşağıdaki metot imzalarını cartItem olarak farklı bir interface de mi tanımlamamız gerekecek ? 
    //Not: Bu soruyu araştırırken aggregation yani birlikte kullanılan ilişkiyi gördüm. Bu yüzden bu metotları ayrı bir interface de tanımlamama gerek yok sanırım. Çünkü cartItem işlemleri cart işlemleri ile birlikte kullanılıyor(CartDto içinde Collection olarak tanımlı). bir nevi zorunluluk.
    Task<ResponseDto<CartItemDto>> AddCartItemAsync(CartItemCreateDto cartItemCreateDto); // Sepete ürün ekleme
    Task<ResponseDto<NoContent>> RemoveFromCartAsync(int cartItemId); // Sepetten ürün çıkarma
    Task<ResponseDto<NoContent>> ClearCartAsync(string applicationUserId); // Sepeti temizleme
    Task<ResponseDto<CartItemDto>> ChangeQuantityAsync(CartItemUpdateDto cartItemUpdateDto); // Sepetteki ürünün adetini değiştirme




    /*
        Task<ResponseDto<NoContent>> UpdateCartItemAsync(CartItemUpdateDto cartItemUpdateDto); // Sepetteki ürünü güncelleme
        Task<ResponseDto<NoContent>> UpdateCartItemQuantityAsync(CartItemUpdateDto cartItemUpdateDto); // Sepetteki ürünün adetini güncelleme işlemi. burda parametre olarak id ve quantity alıyoruz.çünkü id ile ürünü bulup quantity ile güncelleme yapılacaak
        Task<ResponseDto<NoContent>> DeleteCartItemAsync(CartItemDto cartItemDto); // Sepetten ürün silme
        Task<ResponseDto<NoContent>> DeleteAllCartItemAsync(int id); // Sepetteki tüm ürünleri silme
    */
}
