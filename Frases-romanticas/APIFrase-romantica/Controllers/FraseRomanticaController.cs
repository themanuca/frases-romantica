using Microsoft.AspNetCore.Mvc;
using App.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.Entities.Models;
using Contract.DTOs;
namespace APIFrase_romantica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraseRomanticaController : ControllerBase
    {
        private readonly IFraseRomanticaServices _fraseRomanticaServices;

        public FraseRomanticaController(IFraseRomanticaServices fraseRomanticaServices)
        {
            _fraseRomanticaServices = fraseRomanticaServices;
        }

        [HttpGet]
        public async Task<IActionResult> ListarFrases()
        {
            var lista = await _fraseRomanticaServices.ObterTodasAsync();
            if (!lista.Any())
            {
                return NotFound("Frases não encontrada.");
            }
            return Ok(lista);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>ObterFrase(int id)
        {
             var frase = await _fraseRomanticaServices.ObterPorIdAsync(id);
            if(frase?.Texto == null)
            {
                return NotFound();
            }
            
            return Ok(frase);

        }
        [HttpPost]
        public async Task<IActionResult> SalvarFrase(FraseRomantica frase)
        {
            await _fraseRomanticaServices.CriarAsync(frase);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult>AtualizarCurtidas(CurtidasFraseDTO atulizaCurtida)
        {
            await _fraseRomanticaServices.AtualizarCurtidaFrase(atulizaCurtida);
            return Ok();
        }
    }
}
