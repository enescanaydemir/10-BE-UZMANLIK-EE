using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Shared.Dtos.Auth;

public class ResetPasswordDto
{
    public string? Email { get; set; } //Kullanıcının şifresini sıfırlamak için kullanılacak olan email adresi

    [Required(ErrorMessage = "Token alanı bış bırakılamaz")]
    public string? Token { get; set; } //Kullanıcının şifresini sıfırlamak için kullanılacak olan token

    [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
    public string? Password { get; set; } //Kullanıcının yeni şifresi

    [Required(ErrorMessage = "Şifre tekrarı alanı boş bırakılamaz")]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor. Lütfen tekrar deneyiniz.")]
    public string? ConfirmPassword { get; set; } //Kullanıcının yeni şifresini tekrar girmesi için kullanılacak alan
}
