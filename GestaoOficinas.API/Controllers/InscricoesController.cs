using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoOficinas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InscricoesController : ControllerBase
    {
        private readonly IInscricaoService _inscricaoService;

        public InscricoesController(IInscricaoService inscricaoService)
        {
            _inscricaoService = inscricaoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inscricoes = await _inscricaoService.GetAllAsync();
            return Ok(inscricoes);
        }

        [HttpGet("{idAluno}/{idTurma}")]
        public async Task<IActionResult> GetById(int idAluno, int idTurma)
        {
            var inscricao = await _inscricaoService.GetByIdAsync(idAluno, idTurma);
            if (inscricao == null)
            {
                return NotFound();
            }
            return Ok(inscricao);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInscricaoDto dto)
        {
            try
            {
                var inscricaoViewModel = await _inscricaoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { idAluno = inscricaoViewModel.IdAluno, idTurma = inscricaoViewModel.IdTurma }, inscricaoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{idAluno}/{idTurma}")]
        public async Task<IActionResult> Update(int idAluno, int idTurma, [FromBody] UpdateInscricaoDto dto)
        {
            try
            {
                await _inscricaoService.UpdateAsync(idAluno, idTurma, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{idAluno}/{idTurma}")]
        public async Task<IActionResult> Delete(int idAluno, int idTurma)
        {
            try
            {
                await _inscricaoService.DeleteAsync(idAluno, idTurma);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
