using BPROJES�.Models;  // ApplicationUser'�n namespace'i
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BPROJES�.Models;
using BPROJES�.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext ekle (veritaban� ba�lant�s�)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity servislerini ApplicationUser ile ekle
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // �stersen parola ve kullan�c� ayarlar�n� buraya yapabilirsin
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. MVC ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Middleware ayarlar�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 5. Kimlik do�rulama ve yetkilendirme
app.UseAuthentication();  // Kimlik do�rulama aktif
app.UseAuthorization();   // Yetkilendirme aktif

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

builder.Services.AddSession();
app.UseSession();


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Anasayfa}/{action=Anasayfa}/{id?}");
});






if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();