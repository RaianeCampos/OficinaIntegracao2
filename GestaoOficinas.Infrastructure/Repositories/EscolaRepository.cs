using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class EscolaRepository : IEscolaRepository
    {
        private readonly ApplicationDbContext _context;

        public EscolaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Escola escola)
        {
            await _context.Escolas.AddAsync(escola);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CnpjExistsAsync(string cnpj)
        {
            return await _context.Escolas.AnyAsync(e => e.CnpjEscola == cnpj);
        }

        public async Task DeleteAsync(int id)
        {
            var escola = await GetByIdAsync(id);
            if (escola != null)
            {
                _context.Escolas.Remove(escola);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Escola>> GetAllAsync()
        {
            return await _context.Escolas.AsNoTracking().ToListAsync();
        }

        public async Task<Escola> GetByIdAsync(int id)
        {
            return await _context.Escolas.FindAsync(id);
        }

        public async Task UpdateAsync(Escola escola)
        {
            _context.Entry(escola).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}