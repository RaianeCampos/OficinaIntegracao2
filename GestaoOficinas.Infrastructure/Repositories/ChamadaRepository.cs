using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class ChamadaRepository : IChamadaRepository
    {
        private readonly ApplicationDbContext _context;

        public ChamadaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Chamada chamada)
        {
            await _context.Chamadas.AddAsync(chamada);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chamada = await GetByIdAsync(id);
            if (chamada != null)
            {
                _context.Chamadas.Remove(chamada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Chamada>> GetAllAsync()
        {
            return await _context.Chamadas
                .Include(c => c.Turma) 
                .Include(c => c.Aluno) 
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Chamada> GetByIdAsync(int id)
        {
            return await _context.Chamadas
                .Include(c => c.Turma)
                .Include(c => c.Aluno) 
                .FirstOrDefaultAsync(c => c.IdChamada == id);
        }

        public async Task UpdateAsync(Chamada chamada)
        {
            _context.Entry(chamada).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}