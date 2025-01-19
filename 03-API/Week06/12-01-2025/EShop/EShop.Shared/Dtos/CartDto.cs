using System;

namespace EShop.Shared.Dtos;

public class CartDto
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUserDto ApplicationUser { get; set; } = new ApplicationUserDto();
    public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    public decimal TotalAmount => CartItems.Sum(x => x.Product.Price * x.Quantity); //cartItems içindeki her bir elemanın fiyatı ile adedini çarparak toplam fiyatı döndür.
    public int TotalItems => CartItems == null ? 0 : CartItems.Count(); //cartItems boş ise 0 döndür, dolu ise count() ile sayısını döndür.
}




// public decimal TotalPrice => CartItems.Sum(x => x.Product.Price * x.Quantity);

//yukarıdakinin uzun hali;
// public decimal TotalPrice
// {
//     get
//     {
//         return CartItems.Sum(x => x.Product.Price * x.Quantity);
//     }
// }

// public decimal TotalPrice() => CartItems.Sum(x => x.Product.Price * x.Quantity);

//yukarıdakinin uzun hali;
// public decimal TotalPrice()
// {
//     decimal totalPrice = 0;
//     foreach (var item in CartItems)
//     {
//         totalPrice += item.Product.Price * item.Quantity;
//     }
//     return totalPrice;
// }