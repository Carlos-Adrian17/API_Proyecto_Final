using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_banco.Models
{
    public class Movimiento
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Tipo { get; set; } // Depósito, Retiro, Pago, Transferencia

        public decimal Monto { get; set; }

        // 🔑 FOREIGN KEY
        public int CuentaId { get; set; }

        // 🔗 NAVEGACIÓN (IMPORTANTE)
        [ForeignKey("CuentaId")]
        [JsonIgnore]
        public Cuenta Cuenta { get; set; }
    }
}