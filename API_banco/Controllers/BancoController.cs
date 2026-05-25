using Microsoft.AspNetCore.Mvc;
using API_banco.Services;

namespace API_banco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BancoController : ControllerBase
    {
        private readonly BancoService _service;

        public BancoController(BancoService service)
        {
            _service = service;
        }

        // ✅ VALIDAR TARJETA
        [HttpPost("validar")]
        public IActionResult Validar(int cuentaId, decimal monto)
        {
            bool autorizado = _service.ValidarPago(cuentaId, monto);

            return Ok(new
            {
                autorizado = autorizado,
                codigo = autorizado ? "00" : "51",
                mensaje = autorizado ? "Aprobado" : "Fondos insuficientes"
            });
        }

        // ✅ PROCESAR PAGO (LLAMADO POR SERVICIOS)
        [HttpPost("procesar")]
        public IActionResult Procesar(int cuentaId, decimal monto, string servicio)
        {
            try
            {
                var result = _service.ProcesarPago(cuentaId, monto, servicio);

                return Ok(new
                {
                    aprobado = true,
                    codigo = "00",
                    mensaje = result
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    aprobado = false,
                    codigo = "51",
                    mensaje = "Saldo insuficiente"
                });
            }
        }
    }
}
