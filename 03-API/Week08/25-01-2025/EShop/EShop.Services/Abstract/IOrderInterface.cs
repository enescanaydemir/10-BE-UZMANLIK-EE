using System;
using EShop.Shared.ComplexTypes;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.ResponseDtos;

namespace EShop.Services.Abstract
{
    public interface IOrderInterface
    {
        Task<ResponseDto<OrderDto>> GetAsync(int id); // Id'ye göre sipariş getirme
        Task<ResponseDto<IEnumerable<OrderDto>>> GetAllAsync(); //Tüm siparişleri getirme
        Task<ResponseDto<IEnumerable<OrderDto>>> GetAllAsync(OrderStatus? orderStatus); //Tüm siparişleri getirme. parametre olarak sipariş durumuna göre siparişleri getirecek
        Task<ResponseDto<IEnumerable<OrderDto>>> GetAllAsync(string applicationUserId);
        Task<ResponseDto<IEnumerable<OrderDto>>> GetAllAsync(DateTime startDate, DateTime endDate);
        Task<ResponseDto<OrderDto>> AddAsync(OrderCreateDto orderCreateDto); // Sipariş ekleme
        Task<ResponseDto<NoContent>> UpdateOrderStatusAsync(int id, OrderStatus orderStatus); // Sipariş güncelleme
        Task<ResponseDto<NoContent>> CancelOrderAsync(int id); // Siparişi iptal etme
        

    }
}
