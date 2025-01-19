using System.Text;
using EShop.Data.Abstract;
using EShop.Data.Concrete;
using EShop.Data.Concrete.Contexts;
using EShop.Data.Concrete.Repositories;
using EShop.Entity.Concrete;
using EShop.Service.Abstract;
using EShop.Service.Concrete;
using EShop.Shared.Configurations.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
}).AddEntityFrameworkStores<EShopDbContext>().AddDefaultTokenProviders(); // token işlemleri için kullanılır.

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig")); // appsettings.json içerisindeki JwtConfig bölümünü JwtConfig.cs içerisindeki propertylere map eder.

var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig?.Issuer,
        ValidAudience = jwtConfig?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.Secret ?? ""))
    };
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IAuthService, AuthManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
