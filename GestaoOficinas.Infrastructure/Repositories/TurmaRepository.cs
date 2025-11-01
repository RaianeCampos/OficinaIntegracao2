using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly ApplicationDbContext _context;

        public TurmaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Turma turma)
        {
            await _context.Turmas.AddAsync(turma);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var turma = await GetByIdAsync(id);
            if (turma != null)
            {
                _context.Turmas.Remove(turma);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Turma>> GetAllAsync()
        {
            return await _context.Turmas
                .Include(t => t.Oficina)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Turma> GetByIdAsync(int id)
        {
            return await _context.Turmas
                .Include(t => t.Oficina)
                .FirstOrDefaultAsync(t => t.IdTurma == id);
        }

        public async Task UpdateAsync(Turma turma)
        {
            _context.Entry(turma).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}