using System;
using AutoMapper;
using EShop.Data.Abstract;
using EShop.Entity.Concrete;
using EShop.Services.Abstract;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EShop.Services.Concrete;

public class CartManager : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Cart> _cartRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<CartItem> _cartItemRepository;

    public CartManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cartRepository = _unitOfWork.GetRepository<Cart>();
        _productRepository = _unitOfWork.GetRepository<Product>();
        _cartItemRepository = _unitOfWork.GetRepository<CartItem>();
    }

    public async Task<ResponseDto<CartItemDto>> AddToCartAsync(CartItemCreateDto cartItemCreateDto)
    {
        try
        {
            var product = await _productRepository.GetAsync(cartItemCreateDto.ProductId);
            if (product == null)
            {
                return ResponseDto<CartItemDto>.Fail("Ürün bulunamadı", 404);
            }

            //Ürün aktif değilse
            if (!product.IsActive)
            {
                return ResponseDto<CartItemDto>.Fail("Ürün aktif değil", 400);
            }

            // ürün stokta var mı yok mu kontrolü için sepet kontrolü yapacağız;
            var cart = await _cartRepository.GetAsync(
                x => x.Id == cartItemCreateDto.CartId,
                query => query.Include(x => x.CartItems).ThenInclude(ci => ci.Product)
            );
            if (cart == null || cart.CartItems == null)
            {
                return ResponseDto<CartItemDto>.Fail("Sepet bulunamadı", StatusCodes.Status404NotFound);
            }
            //Ürün stokta yoksa kontrolü yapacağız aşağıda;
            var existCartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == cartItemCreateDto.ProductId);

            //Ürün stokta varsa
            if (existCartItem != null)
            {
                existCartItem.Quantity += cartItemCreateDto.Quantity;
                _cartItemRepository.Update(existCartItem);
                var existsResult = await _unitOfWork.SaveAsync();
                if (existsResult < 1)
                {
                    return ResponseDto<CartItemDto>.Fail("Bir sorun oluştu", StatusCodes.Status400BadRequest);
                }
                var existCartItemDto = _mapper.Map<CartItemDto>(existCartItem);
                return ResponseDto<CartItemDto>.Success(existCartItemDto, StatusCodes.Status200OK);
            }

            //Ürün stokta yoksa (her şey  yolundaysa CartItem yaratıldı)
            var cartItem = new CartItem(
                cartItemCreateDto.CartId,
                cartItemCreateDto.ProductId,
                cartItemCreateDto.Quantity
            );

            // await _cartItemRepository.AddAsync(cartItem); (aşağıdakin 2 satırın uzun hali)
            cart.CartItems.Add(cartItem);
            _cartRepository.Update(cart);
            var result = await _unitOfWork.SaveAsync();

            if (result < 1)
            {
                return ResponseDto<CartItemDto>.Fail("Ürün sepete eklenirken bir hata oluştu", StatusCodes.Status500InternalServerError);
            }
            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);
            return ResponseDto<CartItemDto>.Success(cartItemDto, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return ResponseDto<CartItemDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<CartItemDto>> ChangeQuantityAsync(CartItemUpdateDto cartItemUpdateDto)
    {
        try
        {
            var cartItem = await _cartItemRepository.GetAsync(
                x => x.Id == cartItemUpdateDto.Id,
                query => query.Include(x => x.Product)
            ); //cartItemUpdateDto.Id ile cartItem'ı bul ve güncelle
            //cartItem bulunamazsa hata dön
            if (cartItem == null)
            {
                return ResponseDto<CartItemDto>.Fail("Ürün bulunamadı", StatusCodes.Status404NotFound);
            }

            cartItem.Quantity = cartItemUpdateDto.Quantity; //cartItem'ın miktarını güncelle
            _cartItemRepository.Update(cartItem); //cartItem'ı güncelle
            var result = await _unitOfWork.SaveAsync(); //değişiklikleri kaydet
            if (result < 1)
            {
                return ResponseDto<CartItemDto>.Fail("Ürün miktarı güncellenirken bir hata oluştu", StatusCodes.Status500InternalServerError);
            }
            var cartItemDto = _mapper.Map<CartItemDto>(cartItem); //cartItem'ı CartItemDto'ya dönüştür
            return ResponseDto<CartItemDto>.Success(cartItemDto, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<CartItemDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<NoContent>> ClearCartAsync(string applicationUserId)
    {
        try
        {
            var cart = await _cartRepository.GetAsync( //_cartRepository'den GetAsync metodu ile cart'ı bul
                x => x.ApplicationUserId == applicationUserId, //applicationUserId'ye göre cart'ı bul
                query => query.Include(x => x.CartItems) //cart'ı bul ve cartItem'ları ile birlikte getir
            );
            if (cart == null)
            {
                return ResponseDto<NoContent>.Fail("Sepet bulunamadı", StatusCodes.Status404NotFound);
            }
            cart.CartItems?.Clear(); //cartItem'ları temizle
            _cartRepository.Update(cart); //cart'ı güncelle
            var result = await _unitOfWork.SaveAsync(); //değişiklikleri kaydet

            if (result < 1) //save ile result değerini alıp burda result ile kontrol edebiliriz. 
            {
                return ResponseDto<NoContent>.Fail("Sepet temizlenirken bir hata oluştu", StatusCodes.Status500InternalServerError);
            }
            return ResponseDto<NoContent>.Success(StatusCodes.Status204NoContent); //Başarılı ise 204 dön
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<CartDto>> CreateCartAsync(string applicationUserId)
    {
        try
        {
            if (string.IsNullOrEmpty(applicationUserId))
            {
                return ResponseDto<CartDto>.Fail("Kullanıcı id'si boş olamaz", StatusCodes.Status400BadRequest);
            }
            var existCart = await _cartRepository.GetAsync(x => x.ApplicationUserId == applicationUserId); //applicationUserId'ye göre cart var mı diye bak
            if (existCart != null)
            {
                var existCartDto = _mapper.Map<CartDto>(existCart); //cart varsa cart'ı CartDto'ya dönüştür
                return ResponseDto<CartDto>.Success(existCartDto, StatusCodes.Status400BadRequest);
            }
            var cart = new Cart(applicationUserId); //cart yoksa yeni cart oluştur
            await _cartRepository.AddAsync(cart); //cart'ı ekle
            var result = await _unitOfWork.SaveAsync(); //değişiklikleri kaydet
            if (result < 1)
            {
                return ResponseDto<CartDto>.Fail("Sepet oluşturulurken bir hata oluştu", StatusCodes.Status500InternalServerError);
            }
            var cartDto = _mapper.Map<CartDto>(cart); //cart'ı CartDto'ya dönüştür
            return ResponseDto<CartDto>.Success(cartDto, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return ResponseDto<CartDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }

    }

    public async Task<ResponseDto<CartDto>> GetCartAsync(string applicationUserId)
    {
        try
        {
            if (string.IsNullOrEmpty(applicationUserId))
            {
                return ResponseDto<CartDto>.Fail("Kullanıcı bilgisi bulunamadı.", StatusCodes.Status400BadRequest);
            }
            var cart = await _cartRepository.GetAsync(
                x => x.ApplicationUserId == applicationUserId, //applicationUserId'ye göre cart'ı bul
                query => query.Include(x => x.CartItems).ThenInclude(y => y.Product) //cart'ı bul ve cartItem'ları ile birlikte getir. cartItem'ların içindeki product'ları da getir. Yani sepet ve sepetin içindeki ürünleri aldık
            );
            if (cart == null)
            {
                return ResponseDto<CartDto>.Fail("Kullanıcıya ait sepet bulunamadı.", StatusCodes.Status404NotFound);
            }
            var cartDto = _mapper.Map<CartDto>(cart);
            return ResponseDto<CartDto>.Success(cartDto, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<CartDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ResponseDto<NoContent>> RemoveFromCartAsync(int cartItemId) //burda fazladan 2 tane adminin aynı yeri silmeye çalışması gibi bir durumun kontrolünü yapıyoruz
    {
        try
        {
            var cartItem = await _cartItemRepository.GetAsync(cartItemId);
            if (cartItem == null)
            {
                return ResponseDto<NoContent>.Fail("İlgili ürün sepette bulunamadığı için silinemedi", StatusCodes.Status404NotFound);
            }
            _cartItemRepository.Delete(cartItem);
            var result = await _unitOfWork.SaveAsync();
            if (result < 1)
            {
                return ResponseDto<NoContent>.Fail("Bir sorun oluştu", StatusCodes.Status500InternalServerError);
            }
            return ResponseDto<NoContent>.Success(StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}
