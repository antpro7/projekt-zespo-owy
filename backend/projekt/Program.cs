using Microsoft.EntityFrameworkCore;
using projekt.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using projekt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authConfig = builder.Configuration.GetSection("AuthConfig").Get<AuthConfig>();
//get config from secrets
builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("AuthConfig"));

// Konfiguracja bazy danych
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Services
builder.Services.AddTransient<projekt.Services.Interfaces.IAuthService, projekt.Services.AuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Dla developmentu (localhost)

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = authConfig.Issuer,
        ValidAudience = authConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Key)),

        ClockSkew = TimeSpan.FromSeconds(5)
    };
});


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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Projekt API",
        Version = "v1"
    });
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecuritySchemeReference("bearer", doc)
            ]
            = []
    });
});

// Konfiguracja CORS (¿eby Vue mog³o gadaæ z API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();