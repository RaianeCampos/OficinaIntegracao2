using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IInscricaoService
    {
        Task<InscricaoViewModel> GetByIdAsync(int idAluno, int idTurma);
        Task<IEnumerable<InscricaoViewModel>> GetAllAsync();
        Task<InscricaoViewModel> CreateAsync(CreateInscricaoDto dto);
        Task UpdateAsync(int idAluno, int idTurma, UpdateInscricaoDto dto);
        Task DeleteAsync(int idAluno, int idTurma);
    }
}
