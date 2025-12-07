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
    public class AlunosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AlunosControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<Turma> SeedTurmaAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. Criar Escola COMPLETA
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

            // 2. Criar Professor COMPLETO
            var professor = new Professor
            {
                IdProfessor = 1,
                NomeProfessor = "Prof.",
                IdEscola = 1,
                Escola = escola,
                EmailProfessor = "prof@teste.com",
                TelefoneProfessor = "11988888888",
                CargoProfessor = "Docente",
                Representante = false,
                ConhecimentoProfessor = "Testes"
            };

            // 3. Criar Oficina
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

            // 4. Criar Turma
            var turma = new Turma
            {
                IdTurma = 1,
                NomeTurma = "Turma A",
                IdOficina = 1,
                Oficina = oficina,
                PeriodoTurma = "Manhã",
                SemestreTurma = "2025.1"
            };

            await context.Escolas.AddAsync(escola);
            await context.Professores.AddAsync(professor);
            await context.Oficinas.AddAsync(oficina);
            await context.Turmas.AddAsync(turma);

            await context.SaveChangesAsync(); 
            return turma;
        }

        [Fact]
        public async Task Post_CriaNovoAluno_RetornaCreated()
        {
            // Arrange
            var turma = await SeedTurmaAsync(); 
            var novoAluno = new CreateAlunoDto
            {
                NomeAluno = "Aluno Teste",
                EmailAluno = "aluno@teste.com",
                RaAluno = "123456",
                NascimentoAluno = DateTime.Now.AddYears(-18),
                TelefoneAluno = "11777777777",
                TurmaIds = new List<int> { turma.IdTurma }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/alunos", novoAluno);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<AlunoViewModel>();
            viewModel.NomeAluno.Should().Be("Aluno Teste");
        }
    }
}
