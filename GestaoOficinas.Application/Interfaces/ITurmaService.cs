using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface ITurmaService
    {
        Task<TurmaViewModel> GetByIdAsync(int id);
        Task<IEnumerable<TurmaViewModel>> GetAllAsync();
        Task<TurmaViewModel> CreateAsync(CreateTurmaDto dto);
        Task UpdateAsync(int id, UpdateTurmaDto dto);
        Task DeleteAsync(int id);
    }
}
