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

            var escola = new Escola
            {
                IdEscola = 1,
                NomeEscola = "Escola Padrão",
                CnpjEscola = "12345678901234",
                CepEscola = "01001-000",
                RuaEscola = "Rua Teste, 123",
                ComplementoEscola = "Sala 1",
                TelefoneEscola = "11999999999",
                EmailEscola = "escola@teste.com"
            };

            var professor = new Professor
            {
                IdProfessor = 1,
                NomeProfessor = "Prof.",
                IdEscola = 1,
                Escola = escola,
                EmailProfessor = "prof@teste.com",
                TelefoneProfessor = "11988887777",
                ConhecimentoProfessor = "C#, Java",
                Representante = false,
                CargoProfessor = "Docente"
            };

            var oficina = new Oficina
            {
                IdOficina = 1,
                NomeOficina = "Oficina",
                IdProfessor = 1,
                ProfessorResponsavel = professor,
                TemaOficina = "Testes",
                CargaHorariaOficinia = 10,
                DataOficina = DateTime.Now,
                DescricaoOficina = "Desc",
                StatusOficina = "Ativa"
            };

            var turma = new Turma
            {
                IdTurma = 1,
                NomeTurma = "Turma A",
                IdOficina = 1,
                Oficina = oficina,
                PeriodoTurma = "Manhã",
                SemestreTurma = "2025.1"
            };

            await context.AddRangeAsync(escola, professor, oficina, turma);
            await context.SaveChangesAsync();
            return turma;
        }

        [Fact]
        public async Task Post_CriaNovaChamada_RetornaCreated()
        {
            // Arrange
            var turma = await SeedTurmaAsync();
            var novaChamada = new CreateChamadaDto
            {
                DataChamada = DateTime.Today,
                IdTurma = turma.IdTurma
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/chamadas", novaChamada);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<ChamadaViewModel>();
            viewModel.NomeTurma.Should().Be("Turma A");
        }
    }
}
