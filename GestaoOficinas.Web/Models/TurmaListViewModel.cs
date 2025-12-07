using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Web.Models
{
    public class TurmaListViewModel
    {
        public List<TurmaViewModel> Turmas { get; set; } = new List<TurmaViewModel>();
        public string TermoBusca { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}