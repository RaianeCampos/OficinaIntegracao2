using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IEscolaService
    {
        Task<EscolaViewModel> GetByIdAsync(int id);
        Task<IEnumerable<EscolaViewModel>> GetAllAsync();
        Task<EscolaViewModel> CreateAsync(CreateEscolaDto dto);
        Task UpdateAsync(int id, UpdateEscolaDto dto);
        Task DeleteAsync(int id);
    }
}