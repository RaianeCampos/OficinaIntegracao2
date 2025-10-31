using GestaoOficinas.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IPresencaService
    {
        Task<IEnumerable<PresencaDto>> GetPresencasByChamadaAsync(int idChamada);
        Task RegistrarPresencasAsync(RegistrarPresencaDto dto);
        Task UpdatePresencaAsync(int idAluno, int idChamada, bool presente);
    }
}
