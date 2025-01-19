using System;

namespace EShop.Shared.Dtos;

public class OrderCreateDto //OrderCreateDto, OrderDto'nun bir benzeri. OrderDto'dan farkı, OrderDto'da Id, CreatedDate, OrderStatıs, Address, City gibi alanlar varken, OrderCreateDto'da bu alanlar olmayacak. Çünkü OrderCreateDto, OrderDto'ya veri eklemek için kullanılacak. OrderDto ise veri çekmek için kullanılacak.
{
    public string? ApplicationUserId { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    public string? Address { get; set; }
    public string? City { get; set; }
}
