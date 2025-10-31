using GestaoOficinas.Application.DTOs;
using System.Threading.Tasks;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IOficinaTutorService
    {
        Task<OficinaComTutoresDto> GetOficinaComTutoresAsync(int idOficina);
        Task AdicionarTutorAsync(OficinaTutorDto dto);
        Task RemoverTutorAsync(OficinaTutorDto dto);
    }
}
