using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IChamadaService
    {
        Task<ChamadaViewModel> GetByIdAsync(int id);
        Task<IEnumerable<ChamadaViewModel>> GetAllAsync();
        Task<ChamadaViewModel> CreateAsync(CreateChamadaDto dto);
        Task DeleteAsync(int id);
    }
}
