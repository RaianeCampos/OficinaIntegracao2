namespace GestaoOficinas.Application.DTOs
{
    public class EscolaViewModel
    {
        public int IdEscola { get; set; }
        public string NomeEscola { get; set; }
        public string CnpjEscola { get; set; }
        public string EmailEscola { get; set; }
        public string TelefoneEscola { get; set; }
        public string CepEscola { get; set; }
        public string RuaEscola { get; set; }
        public string ComplementoEscola { get; set; }
    }

    public class CreateEscolaDto
    {
        public string NomeEscola { get; set; }
        public string CnpjEscola { get; set; }
        public string CepEscola { get; set; }
        public string RuaEscola { get; set; }
        public string ComplementoEscola { get; set; }
        public string TelefoneEscola { get; set; }
        public string EmailEscola { get; set; }
    }

    public class UpdateEscolaDto
    {
        public int IdEscola { get; set; }
        public string NomeEscola { get; set; }
        public string CnpjEscola { get; set; }
        public string CepEscola { get; set; }
        public string RuaEscola { get; set; }
        public string ComplementoEscola { get; set; }
        public string TelefoneEscola { get; set; }
        public string EmailEscola { get; set; }
    }
}