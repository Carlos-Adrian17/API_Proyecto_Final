using System.Text.Json.Serialization;

namespace API_banco.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }

        // 🔥 SIMULACIÓN DE TARJETA
        public string NumeroTarjeta { get; set; }
        public string CVV { get; set; }

        public int ClienteId { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }
    }
}