using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IProfessorService
    {
        Task<ProfessorViewModel> GetByIdAsync(int id);
        Task<IEnumerable<ProfessorViewModel>> GetAllAsync();
        Task<ProfessorViewModel> CreateAsync(CreateProfessorDto dto);
        Task UpdateAsync(int id, UpdateProfessorDto dto);
        Task DeleteAsync(int id);
    }
}
