using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EShop.MVC.Models
{
    public class LoginModel
    {
        [Display(Name = "Kullanıcı Adı veya Email")] // Display neden kullanılır
        [Required(ErrorMessage = "kullanıcı adı veya Email Boş bırakılamaz")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola Boş bırakılamaz")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}
