using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Microsoft.OpenApi.Models;
using ProyectoPrueba;

using ProyectoPrueba.Services;
using ProyectoPrueba.UnitOfWork;
using ProyectoPrueba.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS CONFIGURACION

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo",
        policy =>
        {
            policy.AllowAnyOrigin()   // Permitir desde cualquier URL (para desarrollo)
                  .AllowAnyMethod()   // Permitir GET, POST, PUT, DELETE
                  .AllowAnyHeader();  // Permitir cualquier encabezado
        });
});

// Configuracion base de datos

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// 1. AGREGA ESTAS DOS LÍNEAS
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

    // 1. Definimos el esquema de seguridad (La cajita para meter el token)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
                      "Ingresa tu token en el campo de texto abajo.\r\n\r\n" +
                      "Ejemplo: '12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // 2. Le decimos a Swagger que use ese esquema en los endpoints
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
            new string[] {}
        }
    });
});


// Inyeccion Dependencia

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AuthService>();
// Agrega esto junto a tus otros servicios (AddControllers, AddDbContext, etc.)
builder.Services.AddMemoryCache();
// 1. Habilitar la autovalidación de FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters(); // Opcional, para clientes web

// 2. Registrar todos los validadores que encuentre en este proyecto
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

app.UseMiddleware<ProyectoPrueba.Middleware.ExceptionMiddleware>();

// 2. AGREGA ESTE BLOQUE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 1. Genera el JSON de la documentación (v1/swagger.json)
    app.UseSwagger();

    // 2. Genera la página web interactiva donde ves los botones
    app.UseSwaggerUI();

    // BORRA ESTO: app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

app.UseAuthentication(); // <--- Agrega esto (¿Quién eres?)
app.UseAuthorization();  // <--- Esto ya lo tenías (¿Qué puedes hacer?)

app.UseAuthorization();

app.MapControllers();

app.Run();
