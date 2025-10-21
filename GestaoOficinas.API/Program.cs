using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Application.Mappers;
using GestaoOficinas.Application.Services;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using GestaoOficinas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração do Banco de Dados (Passo 3) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- 2. Injeção de Dependência (Serviços e Repositórios - Passo 6) ---
builder.Services.AddScoped<IEscolaRepository, EscolaRepository>();
builder.Services.AddScoped<IEscolaService, EscolaService>();

// TODO: Adicionar os outros repositórios e serviços aqui
// builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
// builder.Services.AddScoped<IProfessorService, ProfessorService>();
// ... etc

// --- 3. Configuração do AutoMapper (Passo 6) ---
builder.Services.AddAutoMapper(typeof(MappingProfile));


// --- 4. Configuração de Autenticação JWT (Passo 5) ---
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
    throw new ArgumentNullException("JWT Key, Issuer ou Audience não configurados.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// --- 5. Serviços da API (Controllers e Swagger) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger para aceitar o Token "Bearer"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestaoOficinas.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT: Bearer {seu token}"
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
            new string[] {}
        }
    });
});


// --- Build da Aplicação ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adiciona os middlewares de Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();