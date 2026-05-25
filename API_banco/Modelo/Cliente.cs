using System.Text.Json.Serialization;

namespace API_banco.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DPI { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public List<Cuenta> Cuentas { get; set; }
    }
}