using Microsoft.EntityFrameworkCore;
using projekt.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using projekt;

var builder = WebApplication.CreateBuilder(args);


//get config from secrets
builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("AuthConfig")); 

// Konfiguracja bazy danych
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Services
builder.Services.AddTransient<projekt.Services.Interfaces.IAuthService, projekt.Services.AuthService>();

// KLUCZOWA KONFIGURACJA JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 1. Zapobiega b³êdom 500 przy relacjach (pêtle)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // 2. Wymusza ma³e litery w nazwach pól (np. LeaveType -> leaveType)
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Konfiguracja CORS (¿eby Vue mog³o gadaæ z API)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();