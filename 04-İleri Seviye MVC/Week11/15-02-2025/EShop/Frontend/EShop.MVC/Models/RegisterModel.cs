using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EShop.MVC.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            int year = DateTime.Now.Year - 18;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            DateOfBirth = new DateTime(year, month, day);
        }
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı zorunludur!")]
        [MinLength(5, ErrorMessage = "{0} alanı en az {1} karakter uzunluğunda olmalıdır!")]
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} alanı zorunludur!")]
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı zorunludur!")]
        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        [JsonPropertyName("city")]
        public string City { get; set; } = null!;

        [JsonPropertyName("gender")]
        public Gender Gender { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; } = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} alanı zorunludur!")]
        [EmailAddress(ErrorMessage = "Geçersiz {0} geçersiz bir formattadır!")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} alanı zorunludur!")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "{0} alanı zorunludur!")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;

        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; } = null!;

        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }

    public enum Gender
    {
        None = 1,
        Female = 2,
        Male = 3
    }
}
