using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IDocumentoRepository _repository;
        private readonly IOficinaRepository _oficinaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IEscolaRepository _escolaRepository;
        private readonly IProfessorRepository _professorRepository; // Adicionado para validar quem envia
        private readonly IMapper _mapper;

        public DocumentoService(
            IDocumentoRepository repository,
            IOficinaRepository oficinaRepository,
            IAlunoRepository alunoRepository,
            IEscolaRepository escolaRepository,
            IProfessorRepository professorRepository,
            IMapper mapper)
        {
            _repository = repository;
            _oficinaRepository = oficinaRepository;
            _alunoRepository = alunoRepository;
            _escolaRepository = escolaRepository;
            _professorRepository = professorRepository;
            _mapper = mapper;
        }

        public async Task<DocumentoViewModel> CreateAsync(CreateDocumentoDto dto)
        {
            var professor = await _professorRepository.GetByIdAsync(dto.IdProfessor);
            if (professor == null) throw new Exception("Professor emissor não encontrado.");

            var oficina = await _oficinaRepository.GetByIdAsync(dto.IdOficina);
            if (oficina == null) throw new Exception("Oficina não encontrada.");

            var documento = _mapper.Map<Documento>(dto);
            documento.Emissao = DateTime.UtcNow; // Força UTC
            documento.StatusDocumento = "Gerado";

            string conteudoGerado = "";

            if (dto.TipoDocumento == "Convite")
            {
                conteudoGerado = $"CONVITE OFICIAL\n\n" +
                                 $"O Professor {professor.NomeProfessor} convida para a oficina '{oficina.NomeOficina}'.\n" +
                                 $"Tema Abordado: {oficina.TemaOficina}.\n" +
                                 $"Data: {oficina.DataOficina:dd/MM/yyyy}.";
            }
            else if (dto.TipoDocumento == "Certificado")
            {
                if (dto.IdAluno == null) throw new Exception("Para gerar um Certificado, é necessário selecionar um Aluno.");

                var aluno = await _alunoRepository.GetByIdAsync(dto.IdAluno.Value);
                if (aluno == null) throw new Exception("Aluno não encontrado.");

                string nomeEscola = "Instituição Parceira";
                if (dto.IdEscola != null)
                {
                    var escola = await _escolaRepository.GetByIdAsync(dto.IdEscola.Value);
                    if (escola != null) nomeEscola = escola.NomeEscola;
                }

                conteudoGerado = $"CERTIFICADO DE CONCLUSÃO\n\n" +
                                 $"Certificamos que o aluno {aluno.NomeAluno} concluiu com êxito a oficina de {oficina.NomeOficina}.\n" +
                                 $"Carga Horária: {oficina.CargaHorariaOficinia} horas.\n" +
                                 $"Realização: {nomeEscola}.";
            }
            else if (dto.TipoDocumento == "Convenio") 
            {
                if (dto.IdEscola == null) throw new Exception("Para gerar um Convênio, é necessário selecionar uma Escola.");

                var escola = await _escolaRepository.GetByIdAsync(dto.IdEscola.Value);
                if (escola != null)
                {
                    conteudoGerado = $"TERMO DE CONVÊNIO\n\n" +
                                     $"Estabelece-se a parceria entre o Projeto ELLP e a escola {escola.NomeEscola}\n" +
                                     $"para a realização de atividades extensionistas.\n" +
                                     $"CNPJ: {escola.CnpjEscola}";
                }
            }
            else
            {
                conteudoGerado = $"Documento genérico referente à oficina {oficina.NomeOficina}.";
            }

            documento.ResumoConteudo = conteudoGerado;

            await _repository.AddAsync(documento);
            return _mapper.Map<DocumentoViewModel>(documento);
        }

        public async Task<IEnumerable<DocumentoViewModel>> GetAllAsync()
        {
            var documentos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DocumentoViewModel>>(documentos);
        }

        public async Task<DocumentoViewModel> GetByIdAsync(int id)
        {
            var documento = await _repository.GetByIdAsync(id);
            return documento == null ? null : _mapper.Map<DocumentoViewModel>(documento);
        }

        public async Task UpdateAsync(int id, UpdateDocumentoDto dto)
        {
            var documento = await _repository.GetByIdAsync(id);
            if (documento == null) throw new KeyNotFoundException("Documento não encontrado.");

            _mapper.Map(dto, documento);

            await _repository.UpdateAsync(documento);
        }

        public async Task DeleteAsync(int id)
        {
            var documento = await _repository.GetByIdAsync(id);
            if (documento == null) throw new KeyNotFoundException("Documento não encontrado.");
            await _repository.DeleteAsync(id);
        }
    }
}