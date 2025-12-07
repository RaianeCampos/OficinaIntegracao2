using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Web.Models
{
    public class ProfessorListViewModel
    {
        public List<ProfessorViewModel> Professores { get; set; } = new List<ProfessorViewModel>();
        public string TermoBusca { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}