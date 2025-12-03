namespace GestaoOficinas.Application.DTOs
{
    public class PresencaViewModel
    {
        public int IdChamada { get; set; }
        public List<PresencaAluno> Presencas { get; set; }

    }
    public class PresencaAluno
    {
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; }
        public bool Presente { get; set; }
    }

    public class CreatePresencaDto
    {
        public int IdChamada { get; set; }
        public List<PresencaAluno> Presencas { get; set; }
    }
    public class UpdatePresencaDto
    {
        public int IdChamada { get; set; }
        public List<PresencaAluno> Presencas { get; set; }
    }

}
