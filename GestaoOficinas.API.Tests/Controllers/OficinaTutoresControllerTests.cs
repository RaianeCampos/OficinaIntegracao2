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
    public class OficinaTutoresControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public OficinaTutoresControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        private async Task<(Oficina, Professor)> SeedOficinaEProfessorTutorAsync()
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

            var profResponsavel = new Professor
            {
                IdProfessor = 1,
                NomeProfessor = "Prof. Resp.",
                IdEscola = 1,
                Escola = escola,
                EmailProfessor = "prof@teste.com",
                TelefoneProfessor = "11988887777",
                ConhecimentoProfessor = "C#, Java",
                Representante = false,
                CargoProfessor = "Docente"
            };

            var profTutor = new Professor
            {
                IdProfessor = 2,
                NomeProfessor = "Prof. Tutor",
                IdEscola = 1,
                Escola = escola,
                EmailProfessor = "tutor@teste.com",
                TelefoneProfessor = "11988886666",
                ConhecimentoProfessor = "Python",
                Representante = false,
                CargoProfessor = "Tutor"
            };

            var oficina = new Oficina
            {
                IdOficina = 1,
                NomeOficina = "Oficina",
                IdProfessor = 1,
                ProfessorResponsavel = profResponsavel,
                TemaOficina = "Testes",
                CargaHorariaOficinia = 10,
                DataOficina = DateTime.Now,
                DescricaoOficina = "Desc",
                StatusOficina = "Ativa"
            };

            await context.AddRangeAsync(escola, profResponsavel, profTutor, oficina);
            await context.SaveChangesAsync();
            return (oficina, profTutor);
        }

        [Fact]
        public async Task Post_AdicionarTutor_RetornaOk()
        {
            // Arrange
            var (oficina, tutor) = await SeedOficinaEProfessorTutorAsync();
            var dto = new OficinaTutorDto
            {
                IdOficina = oficina.IdOficina,
                IdProfessor = tutor.IdProfessor
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/oficinatutores", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var relacao = await context.OficinaTutores.FindAsync(oficina.IdOficina, tutor.IdProfessor);
            relacao.Should().NotBeNull();
        }
    }
}
