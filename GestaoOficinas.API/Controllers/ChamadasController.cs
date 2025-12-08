using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoOficinas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChamadasController : ControllerBase
    {
        private readonly IChamadaService _chamadaService;

        public ChamadasController(IChamadaService chamadaService)
        {
            _chamadaService = chamadaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chamadas = await _chamadaService.GetAllAsync();
            return Ok(chamadas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var chamada = await _chamadaService.GetByIdAsync(id);
            if (chamada == null)
            {
                return NotFound();
            }
            return Ok(chamada);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChamadaDto dto)
        {
            try
            {
                var chamadaViewModel = await _chamadaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = chamadaViewModel.IdChamada }, chamadaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateChamadaDto dto)
        {
            try
            {
                // Vamos reutilizar o CreateChamadaDto pois os campos são os mesmos
                await _chamadaService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _chamadaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
