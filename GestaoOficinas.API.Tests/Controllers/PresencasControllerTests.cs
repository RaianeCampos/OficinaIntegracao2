using FluentAssertions;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GestaoOficinas.API.Tests.Controllers
{
    public class PresencasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PresencasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<(Chamada, Aluno)> SeedChamadaEAlunoAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola", CnpjEscola = "123" };
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof.", IdEscola = 1, Escola = escola };
            var oficina = new Oficina { IdOficina = 1, NomeOficina = "Oficina", IdProfessor = 1, ProfessorResponsavel = professor };
            var turma = new Turma { IdTurma = 1, NomeTurma = "Turma A", IdOficina = 1, Oficina = oficina };
            var aluno = new Aluno { IdAluno = 1, NomeAluno = "Aluno Teste", IdTurma = 1, Turma = turma };
            var chamada = new Chamada { IdChamada = 1, IdTurma = 1, Turma = turma };

            await context.AddRangeAsync(escola, professor, oficina, turma, aluno, chamada);
            await context.SaveChangesAsync();
            return (chamada, aluno);
        }

        [Fact]
        public async Task Post_RegistrarPresencas_RetornaOk()
        {
            var (chamada, aluno) = await SeedChamadaEAlunoAsync();
            var registroDto = new RegistrarPresencaDto
            {
                IdChamada = chamada.IdChamada,
                Presencas = new List<PresencaAlunoDto>
                {
                    new PresencaAlunoDto { IdAluno = aluno.IdAluno, Presente = true }
                }
            };

            var response = await _client.PostAsJsonAsync("/api/presencas/registrar", registroDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verificação bônus: checar se foi salvo no banco
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var presenca = await context.Presencas.FindAsync(aluno.IdAluno, chamada.IdChamada);
            presenca.Should().NotBeNull();
            presenca.Presente.Should().BeTrue();
        }
    }
}