using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class InscricaoRepository : IInscricaoRepository
    {
        private readonly ApplicationDbContext _context;
        public InscricaoRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddAsync(Inscricao inscricao)
        {
            await _context.Inscricoes.AddAsync(inscricao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idAluno, int idTurma)
        {
            var inscricao = await GetByIdAsync(idAluno, idTurma);
            if (inscricao != null)
            {
                _context.Inscricoes.Remove(inscricao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Inscricao>> GetAllAsync()
        {
            return await _context.Inscricoes.AsNoTracking().ToListAsync();
        }

        public async Task<Inscricao> GetByIdAsync(int idAluno, int idTurma)
        {
            return await _context.Inscricoes.FindAsync(idAluno, idTurma);
        }

        public async Task UpdateAsync(Inscricao inscricao)
        {
            _context.Entry(inscricao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
