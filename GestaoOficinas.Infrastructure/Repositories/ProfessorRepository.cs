using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly ApplicationDbContext _context;
        public ProfessorRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddAsync(Professor professor)
        {
            await _context.Professores.AddAsync(professor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var professor = await GetByIdAsync(id);
            if (professor != null)
            {
                _context.Professores.Remove(professor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            return await _context.Professores.AsNoTracking().ToListAsync();
        }

        public async Task<Professor> GetByIdAsync(int id)
        {
            return await _context.Professores.FindAsync(id);
        }

        public async Task UpdateAsync(Professor professor)
        {
            _context.Entry(professor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
