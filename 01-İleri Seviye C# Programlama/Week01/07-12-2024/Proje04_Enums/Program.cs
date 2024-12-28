// /*
// OrderStatus:
//     1-> Sipariş Alındı
//     2-> Kargo Hazırlanıyor
//     3-> Kargoya Verildi
//     4-> Teslim Edildi
// */

// //Yeni Sipariş
// int orderStatus=1;

// //Kargo hazırlanıyor
// orderStatus=2;

// //Kargoya verildi
// orderStatus=3;

// //TeslimEdildi
// orderStatus=4;

//Sipariş Alındı
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

OrderStatus? status = OrderStatus.Received;

// //Hazırlanıyor
// status=OrderStatus.Proccesing;
// status=OrderStatus.Shipped;

// System.Console.WriteLine(status);

// Gender gender1 = Gender.Female;
// int gender2=1;


status = OrderStatus.Shipped;
System.Console.WriteLine(status);

string? statusDescription = status
    .GetType()?
    .GetField(status.ToString())?
    .GetCustomAttribute<DescriptionAttribute>()?
    .Description;
System.Console.WriteLine(statusDescription);

status = OrderStatus.Delivered;
string? statusDescription2 = status
    .GetType()?
    .GetField(status.ToString())?
    .GetCustomAttribute<DisplayAttribute>()?
    .Name;
System.Console.WriteLine(statusDescription2);
enum Gender
{
    Female,
    Male
}

enum OrderStatus
{
    [Description("Sipariş Alındı")]
    [Display(Name = "Sipariş Alındı")]
    Received = 1,

    [Description("Hazırlanıyor")]
    [Display(Name = "Hazırlanıyor")]
    Proccesing = 2,

    [Description("Kargoya Verildi")]
    [Display(Name = "Kargoya Verildi")]
    Shipped = 3,

    [Description("Teslim Edildi")]
    [Display(Name = "Teslim Edildi")]
    Delivered = 4
}




// enum OrderStatus
// {
//     Received=1,
//     Proccesing=2,
//     Shipped=3,
//     Delivered=4
// }









// enum OrderStatus
// {
//     Received,//0
//     Proccesing,//1
//     Shipped,//2
//     Delivered//3
// }