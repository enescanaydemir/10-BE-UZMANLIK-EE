using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using EShop.Entity.Concrete;
using EShop.Service.Abstract;
using EShop.Shared.Configurations.Auth;
using EShop.Shared.Dtos;
using EShop.Shared.Dtos.Auth;
using EShop.Shared.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EShop.Service.Concrete;

//IAuthManager interfacinin somut kodlarını yazdığımız sınıf.
public class AuthManager : IAuthService
{
    //Dependency Injection ile UserManager ve SignInManager sınıflarını enjekte ediyoruz.
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private JwtConfig _jwtConfig;
    //Aslında burada başka servisler de olacak, ancak henüz yapmadık.

    public AuthManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtConfig> options)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtConfig = options.Value;
    }

    public Task<ResponseDto<NoContent>> ChangePasswordAsync(ChangePasswordDto resetPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto<NoContent>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto<TokenDto>> LoginAsync(LoginDto loginDto)
    {

        try
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return ResponseDto<TokenDto>.Fail("Kullanıcı adı veya şifre hatalı", StatusCodes.Status400BadRequest);
            }
            var isValidatePassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidatePassword)
            {
                return ResponseDto<TokenDto>.Fail("Kullanıcı adı veya şifre hatalı", StatusCodes.Status400BadRequest);
            }
            var tokenDto = await GenerateJwtToken(user);
            return ResponseDto<TokenDto>.Success(tokenDto, StatusCodes.Status200OK);

        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine($"Giriş yapılırken bir hata oluştu: {ex.Message}");
            throw;
        }
    }

    public async Task<ResponseDto<TokenDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUser != null)
            {
                return ResponseDto<TokenDto>.Fail("Kullanıcı adı zaten kullanılmakta", StatusCodes.Status400BadRequest);
            }
            var user = new ApplicationUser(
                firstName: registerDto.FirstName,
                lastName: registerDto.LastName,
                dateOfBirth: registerDto.DateOfBirth,
                gender: registerDto.Gender
            )
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                EmailConfirmed = true,
                Address = registerDto.Address,
                City = registerDto.City
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password); //passwordu hashlemesi için yukarıda değil burda aldık.
            if (!result.Succeeded)
            {
                return ResponseDto<TokenDto>.Fail("Kullanıcı oluşturulurken bir hata oluştu", StatusCodes.Status400BadRequest);
            }
            result = await _userManager.AddToRoleAsync(user, registerDto.Role); //kullanıcı oluşturulduktan sonra kullanıcıya rol atıyoruz.
            if (!result.Succeeded)
            {
                return ResponseDto<TokenDto>.Fail("Kullanıcı rolü atanırken bir hata oluştu", StatusCodes.Status400BadRequest);
            }

            var userDto = new ApplicationUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                City = user.City,
            };
            return ResponseDto<ApplicationUserDto>.Success(userDto, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Kullanıcı kaydı yapılırken bir hata oluştu: {ex.Message}");
            throw;
        }
    }

    public Task<ResponseDto<NoContent>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        throw new NotImplementedException();
    }

    private async Task<TokenDto> GenerateJwtToken(ApplicationUser user) //burdaki user yukarıdaki değişken olarak tanımlı kontrolünü yaptığımız user
    {
        try
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), //userın idsi ile token oluşturulacak ve bu id ile userın bilgilerine ulaşılacak.
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //tokenın benzersiz olması için guid kullanıyoruz. Guid.NewGuid().ToString() ile benzersiz bir token oluşturuyoruz.
            }.Union(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)); //appsettings.json içerisindeki Secret değerini alıyoruz. Bu değer tokenın şifreleme işlemlerinde kullanılacak.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //tokenın şifreleme işlemlerinde kullanılacak algoritmayı belirliyoruz.
            var expiry = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfig.AccessTokenExpiration)); //tokenın ne kadar süre geçerli olacağını belirliyoruz.

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials
            );

            var tokenDto = new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                AccessTokenExpirationDate = expiry
            };
            return tokenDto;

        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Token oluşturulurken bir hata oluştu: {ex.Message}");

            throw;
        }
    }
}
