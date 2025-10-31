using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class PresencaRepository : IPresencaRepository
    {
        private readonly ApplicationDbContext _context;

        public PresencaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Presenca presenca)
        {
            await _context.Presencas.AddAsync(presenca);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idAluno, int idChamada)
        {
            var presenca = await GetByIdAsync(idAluno, idChamada);
            if (presenca != null)
            {
                _context.Presencas.Remove(presenca);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Presenca> GetByIdAsync(int idAluno, int idChamada)
        {
            return await _context.Presencas
                .FindAsync(idAluno, idChamada);
        }

        public async Task<IEnumerable<Presenca>> GetPresencasByAlunoAsync(int idAluno)
        {
            return await _context.Presencas
                .Where(p => p.IdAluno == idAluno)
                .ToListAsync();
        }

        public async Task<IEnumerable<Presenca>> GetPresencasByChamadaAsync(int idChamada)
        {
            return await _context.Presencas
                .Where(p => p.IdChamada == idChamada)
                .Include(p => p.Aluno) // Inclui dados do aluno
                .ToListAsync();
        }

        public async Task UpdateAsync(Presenca presenca)
        {
            _context.Entry(presenca).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
