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
    public class ProfessoresControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ProfessoresControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<Escola> SeedEscolaAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola Padrão", CnpjEscola = "123" };
            await context.Escolas.AddAsync(escola);
            await context.SaveChangesAsync();
            return escola;
        }

        [Fact]
        public async Task Post_CriaNovoProfessor_RetornaCreated()
        {
            var escola = await SeedEscolaAsync();
            var novoProfessor = new CreateProfessorDto
            {
                NomeProfessor = "Prof. Teste",
                EmailProfessor = "prof@teste.com",
                IdEscola = escola.IdEscola,
                CargoProfessor = "Docente",
                Representante = false
            };

            var response = await _client.PostAsJsonAsync("/api/professores", novoProfessor);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var professorViewModel = await response.Content.ReadFromJsonAsync<ProfessorViewModel>();
            professorViewModel.NomeProfessor.Should().Be("Prof. Teste");
        }

        [Fact]
        public async Task GetById_RetornaProfessor_QuandoProfessorExiste()
        {
            var escola = await SeedEscolaAsync();
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof. Teste", IdEscola = escola.IdEscola };

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Professores.AddAsync(professor);
                await context.SaveChangesAsync();
            }

            var response = await _client.GetAsync("/api/professores/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var professorViewModel = await response.Content.ReadFromJsonAsync<ProfessorViewModel>();
            professorViewModel.IdProfessor.Should().Be(1);
        }
    }
}