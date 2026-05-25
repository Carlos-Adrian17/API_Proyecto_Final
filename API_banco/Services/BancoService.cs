using API_banco.Data;
using API_banco.Models;

namespace API_banco.Services
{
    public class BancoService
    {
        private readonly AppDbContext _context;

        public BancoService(AppDbContext context)
        {
            _context = context;
        }

        // ================= CLIENTE =================
        public Cliente CrearCliente(string nombre, string dpi, string password)
        {
            var cliente = new Cliente
            {
                Nombre = nombre,
                DPI = dpi,
                Password = password
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return cliente;
        }

        public Cliente Login(string dpi, string password)
        {
            var cliente = _context.Clientes
                .FirstOrDefault(c => c.DPI == dpi && c.Password == password);

            if (cliente == null)
                throw new Exception("Credenciales incorrectas");

            return cliente;
        }

        // ================= CUENTA =================
        public Cuenta CrearCuenta(int clienteId)
        {
            var cuenta = new Cuenta
            {
                ClienteId = clienteId,
                NumeroCuenta = Guid.NewGuid().ToString(),
                Saldo = 0,

                // 🔥 TARJETA SIMULADA
                NumeroTarjeta = "4539" + new Random().Next(10000000, 99999999),
                CVV = new Random().Next(100, 999).ToString()
            };

            _context.Cuentas.Add(cuenta);
            _context.SaveChanges();

            return cuenta;
        }

        // ================= SALDO =================
        public decimal ConsultarSaldo(int cuentaId)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no existe");

            return cuenta.Saldo;
        }

        // ================= DEPÓSITO =================
        public void Deposito(int cuentaId, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no existe");

            cuenta.Saldo += monto;

            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = cuentaId,
                Tipo = "Depósito",
                Monto = monto,
                Fecha = DateTime.Now
            });

            _context.SaveChanges();
        }

        // ================= RETIRO =================
        public void Retiro(int cuentaId, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no existe");

            if (cuenta.Saldo < monto)
                throw new Exception("Saldo insuficiente");

            cuenta.Saldo -= monto;

            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = cuentaId,
                Tipo = "Retiro",
                Monto = monto,
                Fecha = DateTime.Now
            });

            _context.SaveChanges();
        }

        // ================= TRANSFERENCIA =================
        public string Transferir(int origenId, int destinoId, decimal monto)
        {
            var origen = _context.Cuentas.Find(origenId);
            var destino = _context.Cuentas.Find(destinoId);

            if (origen == null || destino == null)
                throw new Exception("Cuenta no existe");

            if (origen.Saldo < monto)
                throw new Exception("Saldo insuficiente");

            origen.Saldo -= monto;
            destino.Saldo += monto;

            _context.SaveChanges();

            return "Transferencia realizada";
        }

        // ================= VALIDAR TARJETA =================
        public bool ValidarPago(int cuentaId, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no existe");

            return cuenta.Saldo >= monto;
        }

        // ================= PROCESAR PAGO =================
        public string ProcesarPago(int cuentaId, decimal monto, string servicio)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta.Saldo < monto)
                throw new Exception("Saldo insuficiente");

            cuenta.Saldo -= monto;

            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = cuentaId,
                Tipo = "Pago " + servicio,
                Monto = monto,
                Fecha = DateTime.Now
            });

            _context.SaveChanges();

            return "Pago aprobado";
        }

        // ================= HISTORIAL =================
        public List<Movimiento> ObtenerMovimientos(int cuentaId)
        {
            return _context.Movimientos
                .Where(m => m.CuentaId == cuentaId)
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }
    }
}