using Microsoft.EntityFrameworkCore;
using MyTodo.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// WebAPI ����� ���� ������ ����
builder.Services.AddHttpClient<WebAPIs>(client =>
{
    client.BaseAddress = new Uri("http://localhost:25384/");
});

// Accessor ������ ����
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
