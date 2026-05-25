using Microsoft.AspNetCore.Mvc;
using API_banco.Services;

namespace API_banco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientoController : ControllerBase
    {
        private readonly BancoService _service;

        public MovimientoController(BancoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Obtener(int cuentaId)
        {
            try
            {
                var movimientos = _service.ObtenerMovimientos(cuentaId);
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}