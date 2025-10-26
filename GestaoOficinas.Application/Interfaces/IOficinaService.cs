using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IOficinaService
    {
        Task<OficinaViewModel> GetByIdAsync(int id);
        Task<IEnumerable<OficinaViewModel>> GetAllAsync();
        Task<OficinaViewModel> CreateAsync(CreateOficinaDto dto);
        Task UpdateAsync(int id, UpdateOficinaDto dto);
        Task DeleteAsync(int id);
    }
}
