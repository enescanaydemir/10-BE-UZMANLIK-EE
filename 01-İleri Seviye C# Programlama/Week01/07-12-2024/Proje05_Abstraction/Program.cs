// //Bir Eticaret sistemimiz var. Ürünlerin farklı tipleri olabilir, Elektronik, Giyim, Gıda... Her ürünün temel özellikleri aynı iken, bazı özellikleri tipe göre değişiklik gösterebilir. Bu durumda üst sınıf olarak planlayacağımız Product bir abstract class olacaktır.

//class Product: BaseEntity,IEntity,ICommon
//class Product: ICommon, BaseEntity, IEntity  //Bu yanlış


Electronic electronic1 = new()
{
    Id = 1,
    Name = "Müzik Çalar",
    Price = 600
};
decimal discount1 = electronic1.CalculateDiscountedPrice();
System.Console.WriteLine(discount1);



Clothing clothing1 = new()
{
    Id = 2,
    Name = "Erkek kot pantolon",
    Price = 600
};
decimal discount2 = clothing1.CalculateDiscountedPrice();
System.Console.WriteLine(discount2);

interface IProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public decimal CalculateDiscountedPrice();
}




class Electronic : IProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    public decimal CalculateDiscountedPrice()
    {
        double result = (double)Price * 0.1;
        return Convert.ToDecimal(result);
    }
}

class Clothing : IProduct
{
    public int Id { get;set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    public decimal CalculateDiscountedPrice()
    {
        double result = (double)Price * 0.2 + 1;
        return Convert.ToDecimal(result);
    }
}










// abstract class Product
// {
//     public int Id { get; set; }
//     public string? Name { get; set; }
//     public decimal Price { get; set; }
//     public abstract decimal CalculateDiscountedPrice();
// }

// class Electronic : Product
// {
//     public int WarrantyPeriod { get; set; }

//     public override decimal CalculateDiscountedPrice()
//     {
//         throw new NotImplementedException();
//     }
// }

// class Clothing:Product{
//     public string? Size { get; set; }

//     public override decimal CalculateDiscountedPrice()
//     {
//         throw new NotImplementedException();
//     }
// }

