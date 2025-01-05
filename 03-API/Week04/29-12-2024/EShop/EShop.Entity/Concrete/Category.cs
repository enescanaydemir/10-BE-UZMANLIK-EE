using System;
using EShop.Entity.Abstract;

namespace EShop.Entity.Concrete;

public class Category : BaseEntity
{
    private Category() //EF Core için default constructor oluşturduk. Çünkü EF Core, veritabanına kayıt eklerken default constructor'ı kullanır. Private yaptık. çünkü dışarıdan erişilebilir bir Category değil. Yani dışarıdan new'lenemez. bu yüzden alttaki constructor alınmak zorunda bıraktık.
    {
    }
    public Category(string name, string imageUrl) // neden constructor oluşturduk? Çünkü bir kategorinin adı ve resmi olmak zorunda. Açıklama olabilir olmayabilir. 
    {
        Name = name;
        ImageUrl = imageUrl;
    }

    public string Name { get; set; } = string.Empty; //Empty, null olmayan bir string ifadesi döndürür.
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>(); //Bir kategorinin birden fazla ürünü olabilir. Bu yüzden ProductCategory sınıfından bir liste oluşturduk. HashSet, aynı elemanı bir kez ekler. Yani aynı ürünü bir kategoriye bir kez ekler.

}


//Navigation Property yapılandırma ?