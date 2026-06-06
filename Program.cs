using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Joyeriaoro.Data;
using Joyeriaoro.Models;
using Joyeriaoro.Services;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// CONFIGURAR STRIPE
// ===============================
StripeConfiguration.ApiKey = "sk_test_XXXXXXXXXXXX";

// ===============================
// MVC
// ===============================
builder.Services.AddControllersWithViews();

// ===============================
// BASE DE DATOS
// ===============================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ===============================
// SERVICIOS
// ===============================
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// ===============================
// AUTENTICACIÓN
// ===============================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ===============================
// MIDDLEWARE
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();