/*
    ORM->Object-Relational Mapping
    Entity Framework Core: Dotnet dünyasında kullanılan en yaygın ve başarılı ORM paketi.

    ORM hangi sorunlara çözüm buluyor?:
    * Nesne-Table ilişki uyumsuzluğunu giderme.
    * Soyutlama konusunda da çok büyük imkanlar getiriyor.
    * Veritabanı bağımsızlığı sağlıyor.

    EF Core'ın Temel Bileşenleri:
    1) DbContext: Veri tabanı işlemleri için ana sınıf, EF Core'un kalbi, merkezi
    2) DbSet: Tabloları temsil eden collectionlar.
    3) Entities: Veritabanındaki tabloların C# nesne karşılıkları.
*/

/*
    Projeye EF Core uygulamak için adımlar:
    1) Microsoft.EntityFrameworkCore.SqlServer paketini kur(versiyona dikkat et)
    2) DbContext sınıfından miras alan context sınıfını oluştur. (Genelde AppDbContext, ApplicationDbContext gibi isimler verilir.)
    3) Entity sınıflarını oluştur.(Category, Product, Project)
    4) Context içerisinde her bir entity için DbSet tanımlamalarını yap.
    5) Context içinde OnConfiguring metodunu override ederek ConnectionString bilgisini base class(DbContext)a gönder.
    6) dotnet ef migrations add MigrationName komutu ile migration oluştur.
    7) dotnet ef database update ile ilgili migrationdaki işlemleri veritabanına yansıt.
    */


System.Console.WriteLine();