using Microsoft.AspNetCore.Mvc;
using API_banco.Services;

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

        [HttpPost("registro")]
        public IActionResult Registrar(string nombre, string dpi, string password)
        {
            var cliente = _service.CrearCliente(nombre, dpi, password);
            return Ok(cliente);
        }

        [HttpPost("login")]
        public IActionResult Login(string dpi, string password)
        {
            var cliente = _service.Login(dpi, password);
            return Ok(cliente);
        }
    }
}