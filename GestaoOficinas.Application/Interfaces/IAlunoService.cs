using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IAlunoService
    {
        Task<AlunoViewModel> GetByIdAsync(int id);
        Task<IEnumerable<AlunoViewModel>> GetAllAsync();
        Task<AlunoViewModel> CreateAsync(CreateAlunoDto dto);
        Task UpdateAsync(int id, UpdateAlunoDto dto);
        Task DeleteAsync(int id);
    }
}
