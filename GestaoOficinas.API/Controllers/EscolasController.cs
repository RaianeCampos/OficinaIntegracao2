using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoOficinas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protege todos os endpoints deste controller
    public class EscolasController : ControllerBase
    {
        private readonly IEscolaService _escolaService;

        public EscolasController(IEscolaService escolaService)
        {
            _escolaService = escolaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var escolas = await _escolaService.GetAllAsync();
            return Ok(escolas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var escola = await _escolaService.GetByIdAsync(id);
            if (escola == null)
            {
                return NotFound();
            }
            return Ok(escola);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEscolaDto dto)
        {
            try
            {
                var escolaViewModel = await _escolaService.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = escolaViewModel.IdEscola }, escolaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEscolaDto dto)
        {
            try
            {
                await _escolaService.UpdateAsync(id, dto);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _escolaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}