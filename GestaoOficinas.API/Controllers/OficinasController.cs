using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoOficinas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OficinasController : ControllerBase
    {
        private readonly IOficinaService _oficinaService;

        public OficinasController(IOficinaService oficinaService)
        {
            _oficinaService = oficinaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var oficinas = await _oficinaService.GetAllAsync();
            return Ok(oficinas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var oficina = await _oficinaService.GetByIdAsync(id);
            if (oficina == null)
            {
                return NotFound();
            }
            return Ok(oficina);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOficinaDto dto)
        {
            try
            {
                var oficinaViewModel = await _oficinaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = oficinaViewModel.IdOficina }, oficinaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOficinaDto dto)
        {
            try
            {
                await _oficinaService.UpdateAsync(id, dto);
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
                await _oficinaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
