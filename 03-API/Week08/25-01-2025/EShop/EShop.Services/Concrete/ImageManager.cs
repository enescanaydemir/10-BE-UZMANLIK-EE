using System;
using EShop.Services.Abstract;
using EShop.Shared.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;

namespace EShop.Services.Concrete;

public class ImageManager : IImageService
{
    private readonly string _imageFolderPath;
    public ImageManager() //constructor tanımladık
    {
        _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"); //resimlerin kaydedileceği klasörü belirledik. (wwwroot/images) bunları db de saklayıp ordan çekmiyor muyuz ? harddisk yolunu veriyor.
        //imagefolder path ile ilgili kontrol yazıyoruz aşağıda;
        if (!Directory.Exists(_imageFolderPath)) //eğer böyle bir klasör yoksa
        {
            Directory.CreateDirectory(_imageFolderPath); //klasörü oluştur
        }
    }
    public ResponseDto<NoContent> DeleteImage(string imageUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) // resim yolunun boş olup olmadığını kontrol ediyoruz.
            {
                return ResponseDto<NoContent>.Fail("Resim yolu boş olamaz!", StatusCodes.Status400BadRequest);
            }

            var fileName = Path.GetFileName(imageUrl); //resmin adını alıyoruz
            var fileFullPath = Path.Combine(_imageFolderPath, fileName); //resmin tam yolunu alıyoruz

            if (!File.Exists(fileFullPath)) //eğer resim dosyası yoksa
            {
                return ResponseDto<NoContent>.Fail("Resim dosyası bulunamadı!", StatusCodes.Status404NotFound);
            }

            File.Delete(fileFullPath); //resmi siliyoruz
            return ResponseDto<NoContent>.Success(StatusCodes.Status200OK);

        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<string>> UploadImageAsync(IFormFile image)
    {
        try
        {
            //resim dosyası boş mu kontrolü yapacağız
            if (image == null)
            {
                return ResponseDto<string>.Fail("Resim dosyası boş olamaz!", StatusCodes.Status400BadRequest);
            }
            if (image.Length == 0)
            {
                return ResponseDto<string>.Fail("Resim dosyası 0 byte'tan büyük olmalıdır.", StatusCodes.Status400BadRequest);
            }
            //uzantı kontrolü yapacağız
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            var imageExtension = Path.GetExtension(image.FileName); //resmin uzantısını alıyoruz(örn: .jpg)
            if (!allowedExtensions.Contains(imageExtension)) //eğer burda olmayan bir uzantı varsa
            {
                return ResponseDto<string>.Fail("Geçersiz dosya uzantısı. (.jpg, .jpeg, .png, .bmp, .gif)", StatusCodes.Status400BadRequest);
            }

            //boyut kontrolü yapacağız
            if (image.Length > 5 * 1024 * 1024) //5mb'dan büyükse
            {
                return ResponseDto<string>.Fail("Resim dosyası 5mb'dan büyük olamaz.", StatusCodes.Status400BadRequest);
            }

            //resmin adını belirleyeceğiz
            var fileName = $"{Guid.NewGuid()}{imageExtension}"; //resmin adını guid ile belirledik
            var fileFullPath = Path.Combine(_imageFolderPath, fileName); //

            //resmi kaydedeceğiz
            using (var stream = new FileStream(fileFullPath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return ResponseDto<string>.Success($"/images/{fileName}", StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return ResponseDto<string>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}
