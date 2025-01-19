using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Shared.Dtos.Auth;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email boş bırakılamaz")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string? Email { get; set; } //Kullanıcının şifresini unuttuğunda, şifresini sıfırlamak için kullanılacak olan email adresi
}
