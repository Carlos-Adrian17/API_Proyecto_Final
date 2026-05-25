using System.Text.Json.Serialization;

namespace API_banco.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        public string NumeroCuenta { get; set; }

        public decimal Saldo { get; set; }

        // ✅ TARJETA SIMULADA
        public string NumeroTarjeta { get; set; }
        public string CVV { get; set; }

        // ✅ FK
        public int ClienteId { get; set; }

        // ✅ RELACIÓN
        [JsonIgnore]
        public Cliente Cliente { get; set; }
    }
}