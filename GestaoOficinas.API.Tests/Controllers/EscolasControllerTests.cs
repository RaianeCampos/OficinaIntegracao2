using FluentAssertions;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GestaoOficinas.API.Tests.Controllers
{
    // Este é um exemplo de arquivo de teste para o EscolasController
    public class EscolasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EscolasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Adiciona o cabeçalho de autenticação falso
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("TestScheme");
        }

        [Fact]
        public async Task GetById_EndpointProtegido_RetornaOkQuandoAutenticado()
        {
            // Arrange
            // Adiciona uma escola ao banco em memória
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Escolas.Add(new Escola { IdEscola = 1, NomeEscola = "Escola Teste", CnpjEscola = "123" });
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/escolas/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var escola = await response.Content.ReadFromJsonAsync<EscolaViewModel>();
            escola.NomeEscola.Should().Be("Escola Teste");
        }

        [Fact]
        public async Task GetById_RetornaNotFound_ParaIdInvalido()
        {
            // Arrange
            // Nenhuma escola adicionada

            // Act
            var response = await _client.GetAsync("/api/escolas/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CriaNovaEscola_RetornaCreated()
        {
            // Arrange
            var novaEscola = new CreateEscolaDto
            {
                NomeEscola = "Nova Escola",
                CnpjEscola = "555666",
                EmailEscola = "nova@escola.com",
                CepEscola = "12345",
                RuaEscola = "Rua Teste",
                TelefoneEscola = "999"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/escolas", novaEscola);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }
    }
}
