using Microsoft.AspNetCore.Mvc;
using API_banco.Services;
using API_banco.Models;

namespace API_banco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly BancoService _service;

        public ClienteController(BancoService service)
        {
            _service = service;
        }

        // ✅ REGISTRO
        [HttpPost("registro")]
        public IActionResult Registrar(
            string nombre,
            string dpi,
            string correo,
            string telefono,
            string direccion,
            string password)
        {
            try
            {
                var cliente = _service.CrearCliente(
                    nombre,
                    dpi,
                    correo,
                    telefono,
                    direccion,
                    password);

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ✅ LOGIN (POR DPI)
        [HttpPost("login")]
        public IActionResult Login(string dpi, string password)
        {
            try
            {
                var cliente = _service.Login(dpi, password);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}