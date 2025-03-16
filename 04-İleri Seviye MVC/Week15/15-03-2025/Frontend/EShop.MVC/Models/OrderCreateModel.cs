using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EShop.MVC.Models
{
    public class OrderCreateModel
    {
        [JsonPropertyName("orderItems")]
        public List<OrderItemCreateModel> OrderItems { get; set; } = [];

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Adres alanı zorunludur!")]
        [JsonPropertyName("address")]
        public string Address { get; set; } = "Zeliha hanım mh. Hanönü Cd. Çember Sk. No:10/9";

        [Display(Name = "Şehir")]
        [Required(ErrorMessage = "Şehir alanı zorunludur!")]
        [JsonPropertyName("city")]
        public string City { get; set; } = "İstanbul";
    }
}
