using System;
using AutoMapper;
using Azure;
using EShop.Data.Abstract;
using EShop.Entity.Concrete;
using EShop.Services.Abstract;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.Concrete;

public class ProductManager : IProductService
{
    public ProductManager(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageService = imageService;
        _productRepository = _unitOfWork.GetRepository<Product>();
        _categoryRepository = _unitOfWork.GetRepository<Category>();
    }

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;

    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Category> _categoryRepository;

    public async Task<ResponseDto<ProductDto>> AddAsync(ProductCreateDto productCreateDto)
    {
        try
        {
            //Kategorilerin varlığının kontrol edilmesi
            foreach (var categoryId in productCreateDto.CategoryIds)
            {
                var categoryExists = await _categoryRepository.ExistsAsync(x => x.Id == categoryId && x.IsActive && !x.IsDeleted); //Kategori var mı? Aktif mi? Silinmemiş mi? kontrolleri yazıldı
                //Kategori bulunamadı ise hata döndürüldü
                if (!categoryExists)
                {
                    return ResponseDto<ProductDto>.Fail($"{categoryId} id'li kategori bulunamadı veya pasif/silinmiş.", StatusCodes.Status404NotFound);
                }
            }

            //Ürün ekleme işlemi
            var product = _mapper.Map<Product>(productCreateDto); //ProductCreateDto'dan Product'a dönüşüm yapıldı
            if (productCreateDto.Image == null) // Ürün resmi boş mu?
            {
                return ResponseDto<ProductDto>.Fail("Ürün resmi boş olamaz.", StatusCodes.Status400BadRequest);
            }

            //Ürün resmi yükleme işlemi
            var imageResponse = await _imageService.UploadImageAsync(productCreateDto.Image); //resim yükleme işlemi yapıldı
            if (!imageResponse.IsSuccessful && imageResponse.Error != null) //resim yükleme işlemi başarısız mı? && hata mesajı var mı ?
            {
                return ResponseDto<ProductDto>.Fail(imageResponse.Error, imageResponse.StatusCode);
            }

            //resim yükleme işlemi başarılı ise
            product.ImageUrl = imageResponse.Data; //resim yolu eklendi

            //Ürün ekleme işlemi
            await _productRepository.AddAsync(product);

            //Ürün ekleme işlemi sonrası veritabanına kayıt işlemi
            var result = await _unitOfWork.SaveAsync();

            //Eğer kayıt işlemi başarısız ise hata döndürüldü.
            if (result < 1)
            {
                return ResponseDto<ProductDto>.Fail("Ürün eklenirken bir hata oluştu.", StatusCodes.Status500InternalServerError);
            }

            //Ürün kategorileri ekleme işlemi
            product.ProductCategories =
                [.. productCreateDto
                        .CategoryIds
                        .Select(categoryId => new ProductCategory(product.Id, categoryId))];

            //Ürün kategorileri ekleme işlemi sonrası veritabanına kayıt işlemi
            _productRepository.Update(product); //Ürün güncelleme işlemi yapıldı
            result = await _unitOfWork.SaveAsync(); //Ürün güncelleme işlemi sonrası veritabanına kayıt işlemi
            if (result < 1) //Eğer kayıt işlemi başarısız ise hata döndürüldü.
            {
                return ResponseDto<ProductDto>.Fail("Ürün kategorileri eklenirken bir hata oluştu.", StatusCodes.Status500InternalServerError);
            }

            var response = await GetWithCategoriesAsync(product.Id); //Ürün ve kategorileri getirme işlemi yapıldı
            if (!response.IsSuccessful && response.Error != null) //Ürün ve kategorileri getirme işlemi başarısız ise hata döndürüldü
            {
                return ResponseDto<ProductDto>.Fail(response.Error, response.StatusCode);
            }
            return ResponseDto<ProductDto>.Success(response.Data, StatusCodes.Status201Created); //Ürün ve kategorileri getirme işlemi başarılı ise başarılı dönüş yapıldı
        }
        catch (Exception ex)
        {
            return ResponseDto<ProductDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<int>> CountAsync(bool isActive)
    {
        try
        {
            var count = await _productRepository.CountAsync(x => x.IsActive == isActive);
            return ResponseDto<int>.Success(count, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<int>> CountAsync()
    {
        try
        {
            var count = await _productRepository.CountAsync();
            return ResponseDto<int>.Success(count, StatusCodes.Status200OK);
        }
        catch (System.Exception ex)
        {

            return ResponseDto<int>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllAsync()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            if (!products.Any())
            {
                return ResponseDto<IEnumerable<ProductDto>>.Fail("Herhangi bir ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return ResponseDto<IEnumerable<ProductDto>>.Success(productDtos, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<ProductDto>>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllAsync(bool isActive)
    {
        try
        {
            var products = await _productRepository.GetAllAsync(
                x => x.IsActive == isActive
            );

            if (!products.Any())
            {
                return ResponseDto<IEnumerable<ProductDto>>.Fail("Herhangi bir ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return ResponseDto<IEnumerable<ProductDto>>.Success(productDtos, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<ProductDto>>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllWithCategoriesAsync()
    {
        try
        {
            var products = await _productRepository.GetAllAsync(
                predicate: x => x.IsActive == true,
                orderBy: x => x.OrderByDescending(y => y.CreateDate),
                includes: query => query.Include(x => x.ProductCategories).ThenInclude(x => x.Category)
            );

            if (!products.Any())
            {
                return ResponseDto<IEnumerable<ProductDto>>.Fail("Herhangi bir ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return ResponseDto<IEnumerable<ProductDto>>.Success(productDtos, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<ProductDto>>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<ProductDto>> GetAsync(int id)
    {
        try
        {
            //Ürünün varlığının kontrol edilmesi
            var product = await _productRepository.GetAsync(id); //Id'ye göre ürün getirme işlemi yapıldı
            if (product == null) //Ürün bulunamadı ise hata döndürüldü
            {
                return ResponseDto<ProductDto>.Fail("Ürün bulunamadı.", StatusCodes.Status404NotFound);
            }

            if (!product.IsActive) //Ürün aktif değil ise hata döndürüldü
            {
                return ResponseDto<ProductDto>.Fail("Ürün pasif durumda.", StatusCodes.Status400BadRequest);
            }
            var productDto = _mapper.Map<ProductDto>(product); //Ürün dönüşüm işlemi yapıldı
            return ResponseDto<ProductDto>.Success(productDto, StatusCodes.Status200OK); //Ürün dönüşüm işlemi başarılı ise başarılı dönüş yapıldı
        }
        catch (Exception ex)
        {
            return ResponseDto<ProductDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetByCategoryAsync(int categoryId)
    {
        try
        {
            var products = await _productRepository.GetAllAsync(
                predicate: x => x.IsActive == true && x.ProductCategories.Any(pc => pc.CategoryId == categoryId),
                orderBy: x => x.OrderByDescending(y => y.CreateDate),
                includes: query => query.Include(x => x.ProductCategories).ThenInclude(x => x.Category)
            );

            if (!products.Any())
            {
                return ResponseDto<IEnumerable<ProductDto>>.Fail("Bu kategoride hiçbir ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return ResponseDto<IEnumerable<ProductDto>>.Success(productDtos, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<ProductDto>>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<ProductDto>> GetWithCategoriesAsync(int categoryId)
    {
        try
        {
            var product = await _productRepository.GetAsync(categoryId);
            if (product == null)
            {
                return ResponseDto<ProductDto>.Fail("Ürün bulunamadı", StatusCodes.Status404NotFound);
            }
            if (!product.IsActive)
            {
                return ResponseDto<ProductDto>.Fail("Ürün aktif değil", StatusCodes.Status400BadRequest);
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return ResponseDto<ProductDto>.Success(productDto, StatusCodes.Status200OK);
        }
        catch (System.Exception ex)
        {
            return ResponseDto<ProductDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<NoContent>> HardDeleteAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return ResponseDto<NoContent>.Fail("Ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            _productRepository.Delete(product);
            await _unitOfWork.SaveAsync();
            return ResponseDto<NoContent>.Success(StatusCodes.Status200OK);
        }
        catch (System.Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<NoContent>> SoftDeleteAsync(int id)
    {
        try
        {
            //Ürünün database de varlığının kontrol edilmesi
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return ResponseDto<NoContent>.Fail("Ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            product.IsDeleted = !product.IsDeleted; //Ürünün silinme durumunun tersi alındı. (Silinmiş ise geri getirildi, geri getirilmiş ise silindi)
            if (product.IsDeleted) product.IsActive = false; //Eğer ürün silinmiş ise aktif durumu false yapıldı
            _productRepository.Update(product);
            await _unitOfWork.SaveAsync();
            return ResponseDto<NoContent>.Success(StatusCodes.Status200OK);
        }
        catch (System.Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        try
        {
            var product = await _productRepository.GetAsync(
                predicate: x => x.Id == productUpdateDto.Id,
                includes: query => query.Include(x => x.ProductCategories)
            );
            if (product == null)
            {
                return ResponseDto<NoContent>.Fail("Ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            if (product.IsActive)
            {
                return ResponseDto<NoContent>.Fail("Ürün pasif durumda!", StatusCodes.Status400BadRequest);
            }

            //Kategorilerin varlığının kontrol edilmesi
            foreach (var categoryId in productUpdateDto.CategoryIds)
            {
                var categoryExists = await _categoryRepository.ExistsAsync(x => x.Id == categoryId && x.IsActive == !x.IsDeleted);
                if (!categoryExists)
                {
                    return ResponseDto<NoContent>.Fail($"{categoryId} id'li kategori bulunamadı veya pasif/silinmiş.", StatusCodes.Status400BadRequest);
                }
            }

            if (productUpdateDto.Image != null)
            {
                var imageResponse = await _imageService.UploadImageAsync(productUpdateDto.Image);
                if (!imageResponse.IsSuccessful && imageResponse.Error != null)
                {
                    return ResponseDto<NoContent>.Fail(imageResponse.Error, imageResponse.StatusCode);
                }
                product.ImageUrl = imageResponse.Data; //resim yolu eklendi
            }
            _mapper.Map(productUpdateDto, product); //ProductUpdateDto'dan Product'a dönüşüm yapıldı

            product.ProductCategories.Clear(); //Ürün kategorileri temizlendi(ProductCategories temizledik, yeni datalarla dolduracağız)
            product.ProductCategories =
                [.. productUpdateDto
                        .CategoryIds
                        .Select(categoryId => new ProductCategory(product.Id, categoryId))];
            _productRepository.Update(product); //Ürün güncelleme işlemi yapıldı 
            var result = await _unitOfWork.SaveAsync();
            if (result < 1)
            {
                return ResponseDto<NoContent>.Fail("Ürün güncellenirken bir hata oluştu.", StatusCodes.Status500InternalServerError);
            }
            return ResponseDto<NoContent>.Success(StatusCodes.Status200OK);

        }
        catch (System.Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<bool>> UpdateIsActiveAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetAsync(x => x.Id == id);
            if (product == null)
            {
                return ResponseDto<bool>.Fail("Ürün bulunamadı.", StatusCodes.Status404NotFound);
            }
            product.IsActive = !product.IsActive;
            _productRepository.Update(product);
            var result = await _unitOfWork.SaveAsync();
            if (result < 1)
            {
                return ResponseDto<bool>.Fail("Ürün güncellenirken bir hata oluştu.", StatusCodes.Status500InternalServerError);
            }
            return ResponseDto<bool>.Success(product.IsActive, StatusCodes.Status200OK);
        }
        catch (System.Exception ex)
        {
            return ResponseDto<bool>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}
