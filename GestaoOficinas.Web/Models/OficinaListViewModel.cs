using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Web.Models
{
    public class OficinaListViewModel
    {
        public List<OficinaViewModel> Oficinas { get; set; } = new List<OficinaViewModel>();
        public string TermoBusca { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}