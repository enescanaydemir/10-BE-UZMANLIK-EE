using System;
using EShop.Entity.Abstract;
using EShop.Shared.ComplexTypes;

namespace EShop.Entity.Concrete;

public class Order : BaseEntity
{
    private Order()
    {
    }
    public Order(string? applicationUserId, string? address, string? city) //aşağıda null olabilir olarak tanımlayıp ? eklediğimiz için burada da null olabilir olarak tanımlamamız gerekiyor. ileride tip değişkeni hatası almamak için
    {
        ApplicationUserId = applicationUserId;
        Address = address;
        City = city;
    }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public OrderStatusType OrderStatus { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
