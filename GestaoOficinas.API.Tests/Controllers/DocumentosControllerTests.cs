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
    public class DocumentosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public DocumentosControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<(Oficina, Escola, Aluno)> SeedDadosAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola", CnpjEscola = "123" };
            var professor = new Professor { IdProfessor = 1, NomeProfessor = "Prof.", IdEscola = 1, Escola = escola };
            var oficina = new Oficina { IdOficina = 1, NomeOficina = "Oficina", IdProfessor = 1, ProfessorResponsavel = professor };
            var turma = new Turma { IdTurma = 1, NomeTurma = "Turma A", IdOficina = 1, Oficina = oficina };
            var aluno = new Aluno { IdAluno = 1, NomeAluno = "Aluno Teste", IdTurma = 1, Turma = turma };

            await context.AddRangeAsync(escola, professor, oficina, turma, aluno);
            await context.SaveChangesAsync();
            return (oficina, escola, aluno);
        }

        [Fact]
        public async Task Post_CriaNovoDocumento_RetornaCreated()
        {
            var (oficina, escola, aluno) = await SeedDadosAsync();
            var novoDocumento = new CreateDocumentoDto
            {
                TipoDocumento = "Certificado",
                IdOficina = oficina.IdOficina,
                IdAluno = aluno.IdAluno,
                IdEscola = escola.IdEscola,
                Emissao = DateTime.Today
            };

            var response = await _client.PostAsJsonAsync("/api/documentos", novoDocumento);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var viewModel = await response.Content.ReadFromJsonAsync<DocumentoViewModel>();
            viewModel.NomeOficina.Should().Be("Oficina");
            viewModel.IdAluno.Should().Be(aluno.IdAluno);
        }
    }
}