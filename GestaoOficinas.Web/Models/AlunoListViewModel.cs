using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Web.Models
{
    public class AlunoListViewModel
    {
        public List<AlunoViewModel> Alunos { get; set; } = new List<AlunoViewModel>();
        public string TermoBusca { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}