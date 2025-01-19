using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Shared.Dtos;

public class CartCreateDto
{
    [Required(ErrorMessage = "Kullanıcı id zorunludur.")]
    public string? ApplicationUserId { get; set; }
}
