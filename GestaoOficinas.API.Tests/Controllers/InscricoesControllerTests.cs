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

            var aluno = new Aluno
            {
                IdAluno = 1,
                NomeAluno = "Aluno Teste",
                Turmas = new List<Turma> { turma },
                EmailAluno = "aluno@teste.com",
                TelefoneAluno = "11777777777",
                RaAluno = "123456",
                NascimentoAluno = DateTime.Now.AddYears(-18)
            };

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
            // Arrange
            var (aluno, turma) = await SeedAlunoETurmaAsync();
            var novaInscricao = new CreateInscricaoDto
            {
                IdAluno = aluno.IdAluno,
                IdTurma = turma.IdTurma,
                StatusInscricao = "Aceito"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/inscricoes", novaInscricao);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<InscricaoViewModel>();
            viewModel.NomeAluno.Should().Be("Aluno Teste");
            viewModel.NomeTurma.Should().Be("Turma A");
        }
    }
}
