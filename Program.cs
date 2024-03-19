using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AccountService>();

var app = builder.Build();

var cultureInfo = new CultureInfo("de-DE");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "deleteConfirmed",
    pattern: "Products/DeleteConfirmed/{id?}",
    defaults: new { controller = "Products", action = "DeleteConfirmed" });

app.MapControllerRoute(
    name: "createProduct",
    pattern: "Products/Create",
    defaults: new { controller = "Products", action = "Create" });

app.MapControllerRoute(
    name: "products",
    pattern: "Products/{action=Index}/{id?}", 
    defaults: new { controller = "Products" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
