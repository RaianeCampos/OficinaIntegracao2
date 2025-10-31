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
    public class PresencasController : ControllerBase
    {
        private readonly IPresencaService _presencaService;

        public PresencasController(IPresencaService presencaService)
        {
            _presencaService = presencaService;
        }

        [HttpGet("chamada/{idChamada}")]
        public async Task<IActionResult> GetByChamada(int idChamada)
        {
            var presencas = await _presencaService.GetPresencasByChamadaAsync(idChamada);
            return Ok(presencas);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPresencas([FromBody] RegistrarPresencaDto dto)
        {
            try
            {
                await _presencaService.RegistrarPresencasAsync(dto);
                return Ok("Presenças registradas com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
