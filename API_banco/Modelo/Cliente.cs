using System.Text.Json.Serialization;

namespace API_banco.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string DPI { get; set; }

        // ✅ NUEVOS CAMPOS
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public string Password { get; set; }

        // ✅ RELACIÓN
        [JsonIgnore]
        public List<Cuenta> Cuentas { get; set; }
    }
}