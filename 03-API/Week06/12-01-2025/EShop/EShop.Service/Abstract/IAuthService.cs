using System;
using EShop.Shared.Dtos.Auth;
using EShop.Shared.Dtos.ResponseDtos;

namespace EShop.Service.Abstract;

//servis imzaları burada olacak ve bu interface'i implemente eden sınıfların içinde bu metotlar yazılacak.
public interface IAuthService
{
    Task<ResponseDto<TokenDto>> LoginAsync(LoginDto loginDto); //Kullanıcı girişi yapar
    Task<ResponseDto<TokenDto>> RegisterAsync(RegisterDto registerDto); //Kullanıcı kaydı yapar
    Task<ResponseDto<NoContent>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto); //Şifresini unutan kullanıcıya şifresini sıfırlamak için email gönderir
    Task<ResponseDto<NoContent>> ChangePasswordAsync(ChangePasswordDto resetPasswordDto); //Kullanıcının şifresini sıfırlar
    Task<ResponseDto<NoContent>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto); 

}
