using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace GestaoOficinas.API.Tests
{
    // Esta classe é o coração dos testes de integração.
    // Ela "bota" sua API em memória.
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. Remover o DbContext real (PostgreSQL)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // 2. Adicionar um DbContext de teste (em memória)
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // 3. Adicionar o Handler de Autenticação Falso
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                        "TestScheme", options => { });

            });
        }
    }
}
