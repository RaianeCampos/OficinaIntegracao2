using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly ApplicationDbContext _context;
        public DocumentoRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddAsync(Documento documento)
        {
            await _context.Documentos.AddAsync(documento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var documento = await GetByIdAsync(id);
            if (documento != null)
            {
                _context.Documentos.Remove(documento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Documento>> GetAllAsync()
        {
            return await _context.Documentos.AsNoTracking().ToListAsync();
        }

        public async Task<Documento> GetByIdAsync(int id)
        {
            return await _context.Documentos.FindAsync(id);
        }

        public async Task UpdateAsync(Documento documento)
        {
            _context.Entry(documento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
