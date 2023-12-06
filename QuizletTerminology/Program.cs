using Microsoft.EntityFrameworkCore;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Respository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};Encrypt=False";
builder.Services.AddDbContext<TerminologyDBContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddScoped<ITermRespository, TermRespository>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
