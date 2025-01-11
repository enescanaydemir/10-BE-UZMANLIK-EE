using FakeStoreApiMVC.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ApiSettings konfigüre ediliyor.
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("FakeStoreApi")); //appsettings.json dosyasındaki FakeStoreApi alanını ApiSettings sınıfına bağlıyoruz.

// HttpClient ApiSettings ile konfigüre ediliyor.
builder.Services.AddHttpClient("FakeStoreApi", (ServiceProvider, client) =>
{
    ApiSettings apiSettings = ServiceProvider.GetRequiredService<IOptions<ApiSettings>>().Value; //ApiSettings sınıfını alıyoruz. çünkü BaseUrl alanını kullanacağız.
    client.BaseAddress = new Uri(apiSettings.BaseUrl); //HttpClient sınıfının BaseAddress özelliğine ApiSettings sınıfındaki BaseUrl alanını atıyoruz.
});





var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
