namespace GestaoOficinas.Application.DTOs
{
    public class ChamadaViewModel
    {
        public int IdChamada { get; set; }
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }
        public string NomeTurma { get; set; }
    }

    public class CreateChamadaDto
    {
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }
    }
}
