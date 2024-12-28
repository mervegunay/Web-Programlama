using Kuafor1.Models; // ApplicationDbContext s�n�f� i�in gerekli
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Cookie Authentication i�in

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Veritaban� ba�lant�s�n� ekliyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication ve Authorization hizmetlerini ekleyin
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Giris"; // Giri� yapmayanlar bu sayfaya y�nlendirilecek
        options.LogoutPath = "/Admin/Cikis"; // ��k�� yap�ld���nda y�nlendirilecek sayfa
        options.AccessDeniedPath = "/Home/AccessDenied"; // Yetkisiz eri�im
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication ve Authorization Middleware'lerini aktif hale getirin
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
