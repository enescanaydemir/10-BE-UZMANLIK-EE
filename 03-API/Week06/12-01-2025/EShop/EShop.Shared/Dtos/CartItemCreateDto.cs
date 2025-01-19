using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Shared.Dtos;

public class CartItemCreateDto
{
    [Required(ErrorMessage = "Sepet id zorunludur!")]
    public int CartId { get; set; } //hangi sepete

    [Required(ErrorMessage = "Ürün id zorunludur!")]
    public int ProductId { get; set; } //hangi ürün

    [Required(ErrorMessage = "Ürün adedi zorunludur!")]
    public int Quantity { get; set; } //kaç adet
}
