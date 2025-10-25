namespace GestaoOficinas.Application.DTOs
{
    public class OficinaViewModel
    {
        public int IdOficina { get; set; }
        public string NomeOficina { get; set; }
        public string TemaOficina { get; set; }
        public string StatusOficina { get; set; }
        public int IdProfessor { get; set; }
    }

    public class CreateOficinaDto
    {
        public string NomeOficina { get; set; }
        public string TemaOficina { get; set; }
        public string DescricaoOficina { get; set; }
        public int CargaHorariaOficinia { get; set; }
        public DateTime DataOficina { get; set; }
        public string StatusOficina { get; set; }
        public int IdProfessor { get; set; }
    }

    public class UpdateOficinaDto
    {
        public string NomeOficina { get; set; }
        public string TemaOficina { get; set; }
        public string DescricaoOficina { get; set; }
        public int CargaHorariaOficinia { get; set; }
        public DateTime DataOficina { get; set; }
        public string StatusOficina { get; set; }
        public int IdProfessor { get; set; }
    }
}
