using System;

namespace Proje13_EFCoreIntro;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Properties { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }//Navigation Property
}
