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

        // ✅ VALIDAR PAGO
        [HttpPost("validar")]
        public IActionResult Validar(int cuentaId, decimal monto)
        {
            bool autorizado = _service.ValidarPago(cuentaId, monto);

            return Ok(new
            {
                autorizado,
                codigo = autorizado ? "00" : "51",
                mensaje = autorizado ? "Aprobado" : "Fondos insuficientes"
            });
        }

        // ✅ PROCESAR PAGO
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
            catch
            {
                return BadRequest(new
                {
                    aprobado = false,
                    codigo = "51",
                    mensaje = "Saldo insuficiente"
                });
            }
        }

        // ✅ TRANSFERENCIA
        [HttpPost("transferir")]
        public IActionResult Transferir(int origenId, int destinoId, decimal monto)
        {
            try
            {
                var result = _service.Transferir(origenId, destinoId, monto);

                return Ok(new { mensaje = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
