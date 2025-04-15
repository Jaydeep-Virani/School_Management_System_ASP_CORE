using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using School_Management_System_ASP_CORE.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add Connection String from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🔹 Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 🔹 Configure routes
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Holiday}/{action=ManageHolidays}/{id?}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
