namespace Joyeriaoro.Models
{
    public class Venta
    {
        public int VentaId { get; set; }

        public string ProductoNombre { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }

        public string UsuarioEmail { get; set; } = "";

        public string StripeSessionId { get; set; } = "";
        public long Precio { get; internal set; }
    }
}