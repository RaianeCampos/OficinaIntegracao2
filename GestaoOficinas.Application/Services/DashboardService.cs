using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence; // Precisamos do DbContext para queries diretas
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Application.Services
{
    public class DashboardService : IDashboardService
    {
        // Para queries de contagem, às vezes é mais eficiente
        // usar o DbContext diretamente do que múltiplos repositórios.
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var totalEscolas = await _context.Escolas.CountAsync();
            var totalProfessores = await _context.Professores.CountAsync();
            var totalAlunos = await _context.Alunos.CountAsync();
            var totalOficinas = await _context.Oficinas.CountAsync();
            var totalOficinasEmAndamento = await _context.Oficinas
                .CountAsync(o => o.StatusOficina == "Em Andamento");

            return new DashboardDto
            {
                TotalEscolas = totalEscolas,
                TotalProfessores = totalProfessores,
                TotalAlunos = totalAlunos,
                TotalOficinas = totalOficinas,
                TotalOficinasEmAndamento = totalOficinasEmAndamento
            };
        }
    }
}
