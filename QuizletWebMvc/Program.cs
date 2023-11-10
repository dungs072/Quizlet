
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using QuizletWebMvc.Services;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.Services.Terminology;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://apigateway") });
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITerminologyService, TerminologyService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IAchivement, Achivement>();
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
