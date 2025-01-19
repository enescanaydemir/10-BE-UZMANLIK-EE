using System;
using EShop.Shared.ComplexTypes;

namespace EShop.Shared.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUserDto ApplicationUser { get; set; } = new ApplicationUserDto(); //nesnelerde null referans hatası almamak için new kullanacağız.
    public string? Address { get; set; }
    public string? City { get; set; }
    public OrderStatusType OrderStatıs { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    public decimal TotalAmount => OrderItems.Sum(x => x.TotalPrice); //OrderItems içindeki her bir elemanın fiyatı ile adedini çarparak toplam fiyatı döndür. TotalAmount dedik çünkü orderıtemDto içindeki TotalPrice ile karışmasın diye. bu direkt orderDto'nun toplam fiyatı olacak.

}
