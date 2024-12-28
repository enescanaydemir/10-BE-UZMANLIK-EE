/*
Nullable Types
Bu tipler, bir değerin "YOK" (null) olabilmesine izin veren veri türleridir.Genellikler referans tipler için mümkün olan bir durumda. 
Varsayılan olarak null değer içeremeyen Value Type'ler istenilirse null değer içerebilecek hale getirilebilirler. Bunun için tip adının yanına "?" yazılır.
*/

// int? nullableInt = null;


// if (nullableInt.HasValue)
// {
//     System.Console.WriteLine("Değeri var");
// }
// else
// {
//     System.Console.WriteLine("Değeri yok");
// }

int? nullableValue = 250;
// int? result = nullableValue == null ? 100 : nullableValue;
//Null Coalescing Operator (??)
int result = nullableValue ?? 100;
// System.Console.WriteLine(result);


//Bir veritabanından kullanıcının yaşını alıyoruz, ancak bazı durumlarda bu veri null gelebiliyor.


int userAge = GetUserAge();
if (userAge < 0)
{
    System.Console.WriteLine("Kişinin yaş bilgisi yok!");
}
else
{
    System.Console.WriteLine(userAge);
}


int GetUserAge()
{
    int? age = null;//Bu fake bir veri tabanından yaş çekme kodu
    return age ?? -1;
}

