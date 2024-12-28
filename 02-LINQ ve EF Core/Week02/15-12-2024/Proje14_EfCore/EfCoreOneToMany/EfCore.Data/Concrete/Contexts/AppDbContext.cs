using System;
using EfCore.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Data.Concrete.Contexts;

public class AppDbContext:DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=SINIF115\\SQLEXPRESS;Database=EfCoreDb;User=sa;Password=Qwe123.,;TrustServerCertificate=true");
        base.OnConfiguring(optionsBuilder);
    }
}
