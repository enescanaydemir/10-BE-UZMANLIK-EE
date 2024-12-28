using System;
using Microsoft.EntityFrameworkCore;

namespace Proje13_EFCoreIntro;

public class ApplicationDbContext:DbContext
{
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=SINIF115\\SQLEXPRESS;Database=EfCoreIntroDb;User=sa;Password=Qwe123.,;TrustServerCertificate=true");
        base.OnConfiguring(optionsBuilder);
    }
}
