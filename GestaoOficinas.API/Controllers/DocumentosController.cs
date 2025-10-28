using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoOficinas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentoService _documentoService;

        public DocumentosController(IDocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var documentos = await _documentoService.GetAllAsync();
            return Ok(documentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var documento = await _documentoService.GetByIdAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            return Ok(documento);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentoDto dto)
        {
            try
            {
                var documentoViewModel = await _documentoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = documentoViewModel.IdDocumento }, documentoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentoDto dto)
        {
            try
            {
                await _documentoService.UpdateAsync(id, dto);
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
                await _documentoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
