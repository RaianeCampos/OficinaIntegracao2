using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestaoOficinas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OficinaTutoresController : ControllerBase
    {
        private readonly IOficinaTutorService _oficinaTutorService;

        public OficinaTutoresController(IOficinaTutorService oficinaTutorService)
        {
            _oficinaTutorService = oficinaTutorService;
        }

        [HttpGet("oficina/{idOficina}")]
        public async Task<IActionResult> GetOficinaComTutores(int idOficina)
        {
            try
            {
                var dto = await _oficinaTutorService.GetOficinaComTutoresAsync(idOficina);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarTutor([FromBody] OficinaTutorDto dto)
        {
            await _oficinaTutorService.AdicionarTutorAsync(dto);
            return Ok("Tutor adicionado com sucesso.");
        }

        // Usamos HttpPost para "remover" para manter o DTO no corpo, 
        // mas HttpDelete também seria válido com parâmetros de query.
        [HttpPost("remover")]
        public async Task<IActionResult> RemoverTutor([FromBody] OficinaTutorDto dto)
        {
            await _oficinaTutorService.RemoverTutorAsync(dto);
            return Ok("Tutor removido com sucesso.");
        }
    }
}
