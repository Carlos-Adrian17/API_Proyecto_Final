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

        // =====================================
        // CLIENTES
        // =====================================

        public Cliente CrearCliente(string nombre, string dpi, string correo,
            string telefono, string direccion, string password)
        {
            var cliente = new Cliente
            {
                Nombre = nombre,
                DPI = dpi,
                Correo = correo,
                Telefono = telefono,
                Direccion = direccion,
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


        // =====================================
        // CUENTAS
        // =====================================

        public Cuenta CrearCuenta(int clienteId)
        {
            // Verificar si ya existe cuenta
            var existente = _context.Cuentas
                .FirstOrDefault(c => c.ClienteId == clienteId);

            if (existente != null)
                return existente;

            var cuenta = new Cuenta
            {
                NumeroCuenta = GenerarNumeroCuenta(),
                Saldo = 0,
                NumeroTarjeta = GenerarNumeroTarjeta(),
                CVV = GenerarCVV(),
                ClienteId = clienteId
            };

            _context.Cuentas.Add(cuenta);
            _context.SaveChanges();

            return cuenta;
        }


        public decimal ConsultarSaldo(int cuentaId)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no encontrada");

            return cuenta.Saldo;
        }


        public void Deposito(int cuentaId, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no encontrada");

            cuenta.Saldo += monto;

            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = cuentaId,
                Fecha = DateTime.Now,
                Tipo = "Deposito",
                Monto = monto
            });

            _context.SaveChanges();
        }


        // =====================================
        // TRANSFERENCIAS
        // =====================================

        public string Transferir(int origenId, int destinoId, decimal monto)
        {
            var origen = _context.Cuentas.Find(origenId);
            var destino = _context.Cuentas.Find(destinoId);

            if (origen == null || destino == null)
                throw new Exception("Cuenta inválida");

            if (origen.Saldo < monto)
                throw new Exception("Fondos insuficientes");

            origen.Saldo -= monto;
            destino.Saldo += monto;

            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = origenId,
                Fecha = DateTime.Now,
                Tipo = "Transferencia Enviada",
                Monto = monto
            });

            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = destinoId,
                Fecha = DateTime.Now,
                Tipo = "Transferencia Recibida",
                Monto = monto
            });

            _context.SaveChanges();

            return "Transferencia realizada exitosamente";
        }


        // =====================================
        // PAGOS
        // =====================================

        public bool ValidarPago(int cuentaId, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            return cuenta != null && cuenta.Saldo >= monto;
        }


        public string ProcesarPago(int cuentaId, decimal monto, string servicio)
        {
            var cuenta = _context.Cuentas.Find(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no encontrada");

            if (cuenta.Saldo < monto)
                throw new Exception("Fondos insuficientes");

            cuenta.Saldo -= monto;

            // Guardar pago
            _context.Pagos.Add(new PagoServicio
            {
                CuentaId = cuentaId,
                TipoServicio = servicio,
                Monto = monto,
                Fecha = DateTime.Now
            });

            // Guardar movimiento
            _context.Movimientos.Add(new Movimiento
            {
                CuentaId = cuentaId,
                Fecha = DateTime.Now,
                Tipo = $"Pago {servicio}",
                Monto = monto
            });

            _context.SaveChanges();

            return "Pago realizado correctamente";
        }


        // =====================================
        // MOVIMIENTOS
        // =====================================

        public List<Movimiento> ObtenerMovimientos(int cuentaId)
        {
            return _context.Movimientos
                .Where(m => m.CuentaId == cuentaId)
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }


        public List<PagoServicio> ObtenerPagos(int cuentaId)
        {
            return _context.Pagos
                .Where(p => p.CuentaId == cuentaId)
                .OrderByDescending(p => p.Fecha)
                .ToList();
        }


        // =====================================
        // GENERADORES
        // =====================================

        private string GenerarNumeroCuenta()
        {
            return new Random().Next(10000000, 99999999).ToString();
        }

        private string GenerarNumeroTarjeta()
        {
            return string.Join(" ",
                Enumerable.Range(0, 4)
                .Select(_ => new Random().Next(1000, 9999).ToString()));
        }

        private string GenerarCVV()
        {
            return new Random().Next(100, 999).ToString();
        }
    }
}