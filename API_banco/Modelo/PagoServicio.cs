namespace API_banco.Models
{
    public class PagoServicio
    {
        public int Id { get; set; }
        public string TipoServicio { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }
}