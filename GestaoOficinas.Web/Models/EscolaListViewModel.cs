using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Web.Models
{
    public class EscolaListViewModel
    {
        public IEnumerable<EscolaViewModel> Escolas { get; set; }
        public string TermoBusca { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}