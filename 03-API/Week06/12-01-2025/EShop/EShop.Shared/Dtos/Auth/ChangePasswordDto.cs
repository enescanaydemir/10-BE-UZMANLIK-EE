using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Shared.Dtos.Auth;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Mevcut şifre boş bırakılamaz")]
    public string? CurrentPassword { get; set; } //eski şifre

    [Required(ErrorMessage = "Yeni şifre boş bırakılamaz")]
    public string? NewPassword { get; set; } //yeni şifre

    [Required(ErrorMessage = "Yeni şifre tekrarı boş bırakılamaz")]
    [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor. Lütfen tekrar deneyiniz.")]
    public string? ConfirmPassword { get; set; } // yeni şifre 2. kez girme kontrolü
}
