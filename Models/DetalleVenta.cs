using System.ComponentModel.DataAnnotations;

namespace Joyeriaoro.Models
{
    public class DetalleVenta
    {
        [Key]
        public int DetalleVentaId { get; set; }

        public int VentaId { get; set; }
        public Venta Venta { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }
    }
}