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
    //public ApplicationUserDto ApplicationUser { get; set; }
    // public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    

}
