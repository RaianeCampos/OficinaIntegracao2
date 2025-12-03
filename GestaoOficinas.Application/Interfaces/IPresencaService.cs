using GestaoOficinas.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IPresencaService
    {
        Task<IEnumerable<PresencaViewModel>> GetPresencasByChamadaAsync(int idChamada);
        Task RegistrarPresencasAsync(CreatePresencaDto dto);
        Task UpdatePresencaAsync(int idAluno, int idChamada, bool presente);
    }
}
