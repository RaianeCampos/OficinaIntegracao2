using GestaoOficinas.Application.DTOs;
using System.Collections.Generic;

namespace GestaoOficinas.Application.DTOs
{
    public class OficinaTutorDto
    {
        public int IdOficina { get; set; }
        public int IdProfessor { get; set; }
    }

    public class OficinaComTutoresDto
    {
        public OficinaViewModel Oficina { get; set; }
        public IEnumerable<ProfessorViewModel> Tutores { get; set; }
    }
}
