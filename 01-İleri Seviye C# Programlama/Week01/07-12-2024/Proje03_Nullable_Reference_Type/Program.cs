/*
c#8.0 ve sonrasında, reference türlerini nullable yapısı içinde ele alabilme özelliği eklenmiştir. Bu özellik sayesinde, kod yazarken null değerlerle çalışmayı daha güvenli hale getirebiliriz.

VARSAYILAN DAVRANIŞ
Nullable Reference Types Kapalı:Tüm reference türleri varsayılan olarak nullable'dir.--> Klasik(eski) yaklaşım
Nullable Reference Types Açık: Tüm reference türleri varsayılan olarak non-nullable'dir. -->Yeni yaklaşım
*/

// string? firstName;

// #nullable disable
// Category category = null;

// class Category
// {
//     public string? Name { get; set; }
//     #nullable disable
//     public string Title { get; set; }
// }

// string? nullableString =null;
// string nonNullableString="Merhaba";
// if(nullableString!=null)
//     System.Console.WriteLine(nullableString.Length);

// System.Console.WriteLine(nullableString?.Length);

// System.Console.WriteLine(nonNullableString.Length);


// string? greeting = "Merhaba!";
// if(greeting==null){
//     greeting="Hello!";
// }
//Null Coalasing Assignment Operator
// greeting ??= "Hello!";
// System.Console.WriteLine(greeting);

// if(greeting is not null){

// }


//Null Assertion Operator
// string? firstName = null;
// System.Console.WriteLine(firstName!.Length);

/*
Modern kodlamada bu null kontrol mekanizmalarından yararlanmak oldukça faydalıdır. Kodumuzun daha güvenli ve okunabilir hale gelmesini sağlar.
Aynı zamanda Run Time Hatalarını azaltma konusunda da faydalıdır. Bu da kodunuzun kaliteli kod olmasını sağlayan unsurlardan biridir.
*/
