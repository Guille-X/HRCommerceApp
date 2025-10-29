using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.Models;
using HRCommerceApp.Core.Models.Entities;
using HRCommerceApp.Infrastructure.Data;
using HRCommerceApp.Infrastructure.Repositories;
using HRCommerceApp.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configuracion de la bd
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// CONFIGURACIÓN EXPLÍCITA DE AUTHENTICATION PRIMERO
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(3);
    options.SlidingExpiration = true;
});

// AGREGAMOS IDENTITY
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

//SERVICIOS
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IAuthWebService, AuthWebService>();

// HttpClient para AuthWebService
builder.Services.AddHttpClient<IAuthWebService, AuthWebService>();

// Session configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Pipeline normal...
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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();