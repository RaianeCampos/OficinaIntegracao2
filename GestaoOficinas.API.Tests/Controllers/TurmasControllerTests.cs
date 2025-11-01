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
                NomeOficina = "Oficina Teste",
                IdProfessor = 1,
                ProfessorResponsavel = professor,
                TemaOficina = "Testes",
                CargaHorariaOficinia = 10,
                DataOficina = DateTime.Now,
                DescricaoOficina = "Desc",
                StatusOficina = "Ativa"
            };

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
