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

            var escola = new Escola { IdEscola = 1, NomeEscola = "Escola", CnpjEscola = "123" };
            var profResponsavel = new Professor { IdProfessor = 1, NomeProfessor = "Prof. Resp.", IdEscola = 1, Escola = escola };
            var profTutor = new Professor { IdProfessor = 2, NomeProfessor = "Prof. Tutor", IdEscola = 1, Escola = escola };
            var oficina = new Oficina { IdOficina = 1, NomeOficina = "Oficina", IdProfessor = 1, ProfessorResponsavel = profResponsavel };

            await context.AddRangeAsync(escola, profResponsavel, profTutor, oficina);
            await context.SaveChangesAsync();
            return (oficina, profTutor);
        }

        [Fact]
        public async Task Post_AdicionarTutor_RetornaOk()
        {
            var (oficina, tutor) = await SeedOficinaEProfessorTutorAsync();
            var dto = new OficinaTutorDto
            {
                IdOficina = oficina.IdOficina,
                IdProfessor = tutor.IdProfessor
            };

            var response = await _client.PostAsJsonAsync("/api/oficinatutores", dto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verificação bônus: checar se foi salvo no banco
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var relacao = await context.OficinaTutores.FindAsync(oficina.IdOficina, tutor.IdProfessor);
            relacao.Should().NotBeNull();
        }
    }
}