using Microsoft.AspNetCore.Mvc;
using API_banco.Services;

namespace API_banco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoServicioController : ControllerBase
    {
        private readonly BancoService _service;

        public PagoServicioController(BancoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(int cuentaId)
        {
            var pagos = _service.ObtenerPagos(cuentaId);
            return Ok(pagos);
        }
    }
}