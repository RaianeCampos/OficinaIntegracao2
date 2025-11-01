using FluentAssertions;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GestaoOficinas.API.Tests.Controllers
{
    public class EscolasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope; 
        private readonly ApplicationDbContext _context; 

        public EscolasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

       
        private async Task CleanDatabaseAsync()
        {
            _context.Escolas.RemoveRange(_context.Escolas);
            
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetById_EndpointProtegido_RetornaOkQuandoAutenticado()
        {
            // Arrange
            await CleanDatabaseAsync(); // Limpar o estado de testes anteriores

            var escola = new Escola
            {
                IdEscola = 1,
                NomeEscola = "Escola Teste",
                CnpjEscola = "12345678901234",
                CepEscola = "01001-000",
                RuaEscola = "Rua Teste, 123",
                ComplementoEscola = "Sala 1",
                TelefoneEscola = "11999999999",
                EmailEscola = "escola@teste.com"
            };

            // Usar o _context obtido no construtor
            _context.Escolas.Add(escola);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/escolas/1");

            // Assert
            // O erro anterior era 404. Agora deve ser 200 OK.
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var escolaViewModel = await response.Content.ReadFromJsonAsync<EscolaViewModel>();
            escolaViewModel.NomeEscola.Should().Be("Escola Teste");
        }

        [Fact]
        public async Task GetById_RetornaNotFound_ParaIdInvalido()
        {
            // Arrange
            await CleanDatabaseAsync(); // Garantir que o banco esteja limpo

            // Act
            var response = await _client.GetAsync("/api/escolas/999"); 

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CriaNovaEscola_RetornaCreated()
        {
            // Arrange
            await CleanDatabaseAsync(); 

            var novaEscolaDto = new CreateEscolaDto
            {
                NomeEscola = "Escola Nova",
                CnpjEscola = "98765432109876",
                CepEscola = "02002-000",
                RuaEscola = "Rua Nova, 456",
                ComplementoEscola = "Andar 2",
                TelefoneEscola = "11888888888",
                EmailEscola = "nova.escola@teste.com"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/escolas", novaEscolaDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var escolaViewModel = await response.Content.ReadFromJsonAsync<EscolaViewModel>();
            escolaViewModel.NomeEscola.Should().Be("Escola Nova");
            escolaViewModel.CnpjEscola.Should().Be("98765432109876");
           
        }
    }
}