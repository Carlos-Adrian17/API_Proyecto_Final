using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_banco.Models
{
    public class PagoServicio
    {
        public int Id { get; set; }

        public string TipoServicio { get; set; } // Netflix, Teléfono, Donación

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        // 🔑 FOREIGN KEY
        public int CuentaId { get; set; }

        // 🔗 RELACIÓN
        [ForeignKey("CuentaId")]
        [JsonIgnore]
        public Cuenta Cuenta { get; set; }
    }
}