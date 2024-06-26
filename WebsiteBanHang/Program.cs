﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Interface;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repository;
using WebsiteBanHang.Services;
<<<<<<< HEAD

=======
>>>>>>> 3a1e4bbd70492c8f59afe5690fb62d3770da0b90

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
AddCookie(options =>
{
    options.Cookie.Name = "PhoneCookie";
    options.LoginPath = "/User/Login";
});

builder.Services.AddSession();

var connectionString =
builder.Configuration.GetConnectionString("WebsiteBanHangConnection");
builder.Services.AddDbContext<WebsiteBanHangContext>(options =>
options.UseSqlServer(connectionString));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CatologyRepository>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();


builder.Services.AddSingleton<IVnpayService, VnPayService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
name: "trang-chu",
pattern: "trang-chu",
defaults: new { controller = "Home", action = "Index" });

    endpoints.MapControllerRoute(
name: "lien-he",
pattern: "lien-he",
defaults: new { controller = "Contact", action = "Index" });
    endpoints.MapControllerRoute(
 name: "bai-viet",
 pattern: "bai-viet",
 defaults: new { controller = "Blog", action = "Index" });

    endpoints.MapControllerRoute(
name: "san-pham",
pattern: "san-pham",
defaults: new { controller = "Product", action = "Index" });
    endpoints.MapControllerRoute(
name: "dang-ky",
pattern: "dang-ky",
defaults: new { controller = "User", action = "Register" });

    endpoints.MapControllerRoute(
name: "dang-nhap",
pattern: "dang-nhap",
defaults: new { controller = "User", action = "Login" });

    endpoints.MapControllerRoute(
name: "thong-tin",
pattern: "thong-tin",
defaults: new { controller = "User", action = "Info" });

    endpoints.MapControllerRoute(
name: "dang-xuat",
pattern: "dang-xuat",
defaults: new { controller = "User", action = "Logout" });

    endpoints.MapControllerRoute(
 name: "gio-hang",
 pattern: "gio-hang",
 defaults: new { controller = "Cart", action = "Index" });

    endpoints.MapControllerRoute(
 name: "them-gio-hang",
 pattern: "them-gio-hang",
 defaults: new { controller = "Cart", action = "AddItem" });

    endpoints.MapControllerRoute(
name: "the-loai-san-pham",
pattern: "{slug}-{id}",
defaults: new { controller = "Product", action = "CateProd" });

    endpoints.MapControllerRoute(
name: "chi-tiet-san-pham",
pattern: "san-pham/{slug}-{id}",
defaults: new { controller = "Product", action = "ProdDetail" });


    endpoints.MapControllerRoute(
 name: "chuong-trinh",
 pattern: "chuong-trinh/{slug}",
 defaults: new { controller = "Product", action = "Index" });

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
