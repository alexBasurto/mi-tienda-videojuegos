using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApiTiendaVideojuegos.Filters;
using WebApiTiendaVideojuegos.Models;
using WebApiTiendaVideojuegos.Services;
using System.Text;
using WebApiTiendaVideojuegos.Interfaces;
using WebApiTiendaVideojuegos.Validators;
using WebApiTiendaVideojuegos.Interface;

var builder = WebApplication.CreateBuilder(args);

// A�adir contexto de bbdd MiTiendaVideojuegos
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

string connectionString;
string claveJWT;

// Obteniendo el valor de ASPNETCORE_ENVIRONMENT para verificar el entorno
bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

// Obteniendo las variables desde el entorno
connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
claveJWT = Environment.GetEnvironmentVariable("CLAVE_JWT");

// Si alguna variable de entorno no se carga, puedes manejar el error de esta forma
if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(claveJWT))
{
    throw new InvalidOperationException("Variables de entorno necesarias no encontradas.");
}

builder.Services.AddDbContext<MiTiendaVideojuegosContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Aplicar una CORS Policy al proyecto que acepte cualquier petici�n desde cualquier lugar.
// A�adimos CORS

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (isDevelopment)
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins("https://playzone.basurto.dev")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddScoped<IUsuarioValidator, UsuarioValidator>();
builder.Services.AddScoped<IGestorArchivos, GestorArchivos>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroDeExcepcion));
}).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(claveJWT))
               });

// Configurar seguridad en Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
