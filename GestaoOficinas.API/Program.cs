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

// --- 1. Configuração do Banco de Dados ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- 2. Injeção de Dependência (Serviços e Repositórios) ---

// Repositórios
builder.Services.AddScoped<IEscolaRepository, EscolaRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IOficinaRepository, OficinaRepository>();
builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<IInscricaoRepository, InscricaoRepository>();
builder.Services.AddScoped<IChamadaRepository, ChamadaRepository>();
// Repositório de Presença quando criar o serviço
// builder.Services.AddScoped<IPresencaRepository, PresencaRepository>(); 
builder.Services.AddScoped<IDocumentoRepository, DocumentoRepository>();

// Serviços
builder.Services.AddScoped<IEscolaService, EscolaService>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IOficinaService, OficinaService>();
builder.Services.AddScoped<ITurmaService, TurmaService>();
builder.Services.AddScoped<IInscricaoService, InscricaoService>();
builder.Services.AddScoped<IChamadaService, ChamadaService>();
// Serviço de Presença
// builder.Services.AddScoped<IPresencaService, PresencaService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();


// --- 3. Configuração do AutoMapper  ---
builder.Services.AddAutoMapper(typeof(MappingProfile));


// --- 4. Configuração de Autenticação JWT ---
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
