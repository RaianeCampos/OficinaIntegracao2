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
    public class TurmasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public TurmasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<Oficina> SeedOficinaAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola", CnpjEscola = "123" };
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof.", IdEscola = 1, Escola = escola };
            var oficina = new Oficina { IdOficina = 1, NomeOficina = "Oficina Teste", IdProfessor = 1, ProfessorResponsavel = professor };

            await context.Escolas.AddAsync(escola);
            await context.Professores.AddAsync(professor);
            await context.Oficinas.AddAsync(oficina);
            await context.SaveChangesAsync();
            return oficina;
        }

        [Fact]
        public async Task Post_CriaNovaTurma_RetornaCreated()
        {
            var oficina = await SeedOficinaAsync();
            var novaTurma = new CreateTurmaDto
            {
                NomeTurma = "Turma A",
                PeriodoTurma = "Manhã",
                SemestreTurma = "2025.1",
                IdOficina = oficina.IdOficina
            };

            var response = await _client.PostAsJsonAsync("/api/turmas", novaTurma);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<TurmaViewModel>();
            viewModel.NomeOficina.Should().Be("Oficina Teste");
        }
    }
}