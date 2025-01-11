using EShop.Data.Abstract;
using EShop.Data.Concrete;
using EShop.Data.Concrete.Contexts;
using EShop.Data.Concrete.Repositories;
using EShop.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//şifre uzunluğu, karakter sayısı vs ler burada belirlenecek. User için hangi entity, role için hangi entity kullanılacak belirlenecek.
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{ //false true işleri aç kapa gibi düşünebiliriz
    options.Password.RequireDigit = true; // şifrede en az bir rakam olmalı.
    options.Password.RequireLowercase = true; // şifrede en az bir küçük harf olmalı.
    options.Password.RequireNonAlphanumeric = true; // şifrede en az bir alfanümerik olmayan karakter olmalı.
    options.Password.RequireUppercase = true; // şifrede en az bir büyük harf olmalı.
    options.Password.RequiredLength = 8; // şifre en az 8 karakter olmalı.

    options.User.RequireUniqueEmail = true; // aynı email adresi ile birden fazla kullanıcı oluşturulamaz. false yaparask aynı mailden 2-3 kere kayıt oluşturulabilir.
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
