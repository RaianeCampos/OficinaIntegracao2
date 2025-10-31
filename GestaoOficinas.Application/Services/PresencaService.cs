using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Application.Services
{
    public class PresencaService : IPresencaService
    {
        private readonly IPresencaRepository _presencaRepository;
        private readonly IMapper _mapper;

        public PresencaService(IPresencaRepository presencaRepository, IMapper mapper)
        {
            _presencaRepository = presencaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PresencaDto>> GetPresencasByChamadaAsync(int idChamada)
        {
            var presencas = await _presencaRepository.GetPresencasByChamadaAsync(idChamada);
            return _mapper.Map<IEnumerable<PresencaDto>>(presencas);
        }

        public async Task RegistrarPresencasAsync(RegistrarPresencaDto dto)
        {
            foreach (var presencaAluno in dto.Presencas)
            {
                var presencaExistente = await _presencaRepository.GetByIdAsync(presencaAluno.IdAluno, dto.IdChamada);

                if (presencaExistente != null)
                {
                    // Atualiza
                    presencaExistente.Presente = presencaAluno.Presente;
                    await _presencaRepository.UpdateAsync(presencaExistente);
                }
                else
                {
                    // Insere
                    var novaPresenca = new Presenca
                    {
                        IdAluno = presencaAluno.IdAluno,
                        IdChamada = dto.IdChamada,
                        Presente = presencaAluno.Presente
                    };
                    await _presencaRepository.AddAsync(novaPresenca);
                }
            }
        }

        public async Task UpdatePresencaAsync(int idAluno, int idChamada, bool presente)
        {
            var presenca = await _presencaRepository.GetByIdAsync(idAluno, idChamada);
            if (presenca == null)
            {
                throw new KeyNotFoundException("Registro de presença não encontrado.");
            }
            presenca.Presente = presente;
            await _presencaRepository.UpdateAsync(presenca);
        }
    }
}
