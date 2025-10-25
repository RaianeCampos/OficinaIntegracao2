using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class OficinaRepository : IOficinaRepository
    {
        private readonly ApplicationDbContext _context;
        public OficinaRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddAsync(Oficina oficina)
        {
            await _context.Oficinas.AddAsync(oficina);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var oficina = await GetByIdAsync(id);
            if (oficina != null)
            {
                _context.Oficinas.Remove(oficina);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Oficina>> GetAllAsync()
        {
            return await _context.Oficinas.AsNoTracking().ToListAsync();
        }

        public async Task<Oficina> GetByIdAsync(int id)
        {
            return await _context.Oficinas.FindAsync(id);
        }

        public async Task UpdateAsync(Oficina oficina)
        {
            _context.Entry(oficina).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
