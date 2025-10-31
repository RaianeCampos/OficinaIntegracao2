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
    public class InscricoesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public InscricoesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<(Aluno, Turma)> SeedAlunoETurmaAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola", CnpjEscola = "123" };
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof.", IdEscola = 1, Escola = escola };
            var oficina = new Oficina { IdOficina = 1, NomeOficina = "Oficina", IdProfessor = 1, ProfessorResponsavel = professor };
            var turma = new Turma { IdTurma = 1, NomeTurma = "Turma A", IdOficina = 1, Oficina = oficina };
            var aluno = new Aluno { IdAluno = 1, NomeAluno = "Aluno Teste", IdTurma = 1, Turma = turma };

            await context.AddAsync(escola);
            await context.AddAsync(professor);
            await context.AddAsync(oficina);
            await context.AddAsync(turma);
            await context.AddAsync(aluno);
            await context.SaveChangesAsync();
            return (aluno, turma);
        }

        [Fact]
        public async Task Post_CriaNovaInscricao_RetornaCreated()
        {
            var (aluno, turma) = await SeedAlunoETurmaAsync();
            var novaInscricao = new CreateInscricaoDto
            {
                IdAluno = aluno.IdAluno,
                IdTurma = turma.IdTurma,
                StatusInscricao = "Aceito"
            };

            var response = await _client.PostAsJsonAsync("/api/inscricoes", novaInscricao);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<InscricaoViewModel>();
            viewModel.NomeAluno.Should().Be("Aluno Teste");
            viewModel.NomeTurma.Should().Be("Turma A");
        }
    }
}