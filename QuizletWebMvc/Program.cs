
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using QuizletWebMvc.Services;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.Services.Terminology;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://apigateway") });
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITerminologyService, TerminologyService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();
