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
    public class ProfessoresControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;

        public ProfessoresControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

           
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

      
        private async Task CleanDatabaseAsync()
        {
            
            _context.Professores.RemoveRange(_context.Professores);
            _context.Escolas.RemoveRange(_context.Escolas);
            await _context.SaveChangesAsync();
        }

       
        private async Task<Escola> SeedEscolaAsync()
        {
            
            var escola = await _context.Escolas.FindAsync(1);
            if (escola == null)
            {
                
                escola = new Escola
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
                await _context.Escolas.AddAsync(escola);
                await _context.SaveChangesAsync();
            }
            
            return escola;
        }

        [Fact]
        public async Task Post_CriaNovoProfessor_RetornaCreated()
        {
            // Arrange
            await CleanDatabaseAsync(); 
            var escola = await SeedEscolaAsync(); 

            var novoProfessor = new CreateProfessorDto
            {
                NomeProfessor = "Prof. Teste",
                EmailProfessor = "prof@teste.com",
                TelefoneProfessor = "11988888888", 
                ConhecimentoProfessor = "Teste", 
                Representante = false,
                CargoProfessor = "Docente",
                IdEscola = escola.IdEscola
            };

            var response = await _client.PostAsJsonAsync("/api/professores", novoProfessor);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var professorViewModel = await response.Content.ReadFromJsonAsync<ProfessorViewModel>();
            professorViewModel.NomeProfessor.Should().Be("Prof. Teste");
        }

        [Fact]
        public async Task GetById_RetornaProfessor_QuandoProfessorExiste()
        {
            // Arrange
            await CleanDatabaseAsync(); 
            var escola = await SeedEscolaAsync(); 

            var professor = new Professor
            {
                IdProfessor = 1,
                NomeProfessor = "Prof. Teste",
                IdEscola = escola.IdEscola,
                EmailProfessor = "prof@teste.com",
                TelefoneProfessor = "11988888888", 
                ConhecimentoProfessor = "Teste", 
                Representante = false,
                CargoProfessor = "Docente"
            };

            await _context.Professores.AddAsync(professor);
            await _context.SaveChangesAsync();


            var response = await _client.GetAsync("/api/professores/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var professorViewModel = await response.Content.ReadFromJsonAsync<ProfessorViewModel>();
            professorViewModel.IdProfessor.Should().Be(1);
        }
    }
}

