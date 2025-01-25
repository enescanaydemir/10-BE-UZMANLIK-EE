using System;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.ResponseDtos;

namespace EShop.Services.Abstract
{
    public interface IProductService
    {
        Task<ResponseDto<ProductDto>> GetAsync(int id); // Id'ye göre ürün getirme
        Task<ResponseDto<IEnumerable<ProductDto>>> GetAllAsync(); // Tüm ürünleri getirme
        Task<ResponseDto<IEnumerable<ProductDto>>> GetAllAsync(bool? isActive); // Aktif olan ürünleri getirme
        Task<ResponseDto<IEnumerable<ProductDto>>> GetByCategoryAsync(int categoryId); // Kategoriye göre ürünleri getirmek için!!!!
        Task<ResponseDto<ProductDto>> AddAsync(ProductCreateDto productCreateDto); // Ürün ekleme
        Task<ResponseDto<NoContent>> UpdateAsync(ProductUpdateDto productUpdateDto); // Update işlemi için sadece yanıt döndürecek bu yüzden NoContent tipinde tanımladık.
        Task<ResponseDto<NoContent>> SoftDeleteAsync(int id); // Ürünü silme
        Task<ResponseDto<NoContent>> HardDeleteAsync(int id); // Ürünü veritabanından silme
        // Ürün sayısını döndüren metotlar
        Task<ResponseDto<int>> CountAsync(); // Tüm ürünlerin sayısını döndürecek
        Task<ResponseDto<int>> CountAsync(bool? isActive); // Aktif olan ürünlerin sayısını döndürecek
        Task<ResponseDto<bool>> UpdateIsActiveAsync(int id); // Ürünün aktif/pasif durumunu güncelleyecek

    }
}

//stok kontrolü için getInStockAsync eklenebilir,
