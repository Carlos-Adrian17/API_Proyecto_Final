using Microsoft.AspNetCore.Mvc;
using API_banco.Services;

namespace API_banco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly BancoService _service;

        public CuentaController(BancoService service)
        {
            _service = service;
        }

        [HttpPost("crear")]
        public IActionResult Crear(int clienteId)
        {
            try
            {
                var cuenta = _service.CrearCuenta(clienteId);
                return Ok(cuenta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("deposito")]
        public IActionResult Deposito(int cuentaId, decimal monto)
        {
            try
            {
                _service.Deposito(cuentaId, monto);
                return Ok(new { mensaje = "Depósito exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("saldo")]
        public IActionResult Saldo(int cuentaId)
        {
            try
            {
                var saldo = _service.ConsultarSaldo(cuentaId);
                return Ok(new { saldo = saldo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
