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
    public class ChamadasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ChamadasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<Turma> SeedTurmaAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola", CnpjEscola = "123" };
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof.", IdEscola = 1, Escola = escola };
            var oficina = new Oficina { IdOficina = 1, NomeOficina = "Oficina", IdProfessor = 1, ProfessorResponsavel = professor };
            var turma = new Turma { IdTurma = 1, NomeTurma = "Turma A", IdOficina = 1, Oficina = oficina };

            await context.AddRangeAsync(escola, professor, oficina, turma);
            await context.SaveChangesAsync();
            return turma;
        }

        [Fact]
        public async Task Post_CriaNovaChamada_RetornaCreated()
        {
            var turma = await SeedTurmaAsync();
            var novaChamada = new CreateChamadaDto
            {
                DataChamada = DateTime.Today,
                IdTurma = turma.IdTurma
            };

            var response = await _client.PostAsJsonAsync("/api/chamadas", novaChamada);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<ChamadaViewModel>();
            viewModel.NomeTurma.Should().Be("Turma A");
        }
    }
}