namespace GestaoOficinas.Web.Models
{
    public class ChamadaResumoViewModel
    {
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }
        public string NomeTurma { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalAlunos { get; set; }
    }
}