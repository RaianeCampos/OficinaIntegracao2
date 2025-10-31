using FluentAssertions;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GestaoOficinas.API.Tests.Controllers
{
    public class OficinasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public OficinasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<Professor> SeedProfessorAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola Padrão", CnpjEscola = "123" };
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof. Responsável", IdEscola = 1, Escola = escola };

            await context.Escolas.AddAsync(escola);
            await context.Professores.AddAsync(professor);
            await context.SaveChangesAsync();
            return professor;
        }

        [Fact]
        public async Task Post_CriaNovaOficina_RetornaCreated()
        {
            
            var professor = await SeedProfessorAsync();
            var novaOficina = new CreateOficinaDto
            {
                NomeOficina = "Oficina de Testes",
                TemaOficina = "Testes",
                CargaHorariaOficinia = 10,
                DataOficina = DateTime.Now,
                IdProfessor = professor.IdProfessor,
                StatusOficina = "Em Andamento"
            };

            
            var response = await _client.PostAsJsonAsync("/api/oficinas", novaOficina);

            
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<OficinaViewModel>();
            viewModel.NomeOficina.Should().Be("Oficina de Testes");
            viewModel.NomeProfessorResponsavel.Should().Be("Prof. Responsável"); // Testa o mapping
        }
    }
}