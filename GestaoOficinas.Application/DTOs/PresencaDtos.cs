namespace GestaoOficinas.Application.DTOs
{
    public class PresencaDto
    {
        public int IdAluno { get; set; }
        public int IdChamada { get; set; }
        public bool Presente { get; set; }
        public string NomeAluno { get; set; }
    }

    public class RegistrarPresencaDto
    {
        public int IdChamada { get; set; }
        public List<PresencaAlunoDto> Presencas { get; set; }
    }

    public class PresencaAlunoDto
    {
        public int IdAluno { get; set; }
        public bool Presente { get; set; }
    }
}
