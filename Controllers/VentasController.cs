using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Joyeriaoro.Data;
using Joyeriaoro.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace Joyeriaoro.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR VENTAS
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ventas.ToListAsync());
        }

        // CREAR VENTA
        public async Task<IActionResult> Create()
        {
            ViewBag.Productos = await _context.Productos.ToListAsync();
            return View();
        }

        public async Task<IActionResult> Factura(int id)
        {
            var venta = await _context.Ventas
                .FirstOrDefaultAsync(v => v.VentaId == id);

            var detalles = await _context.DetalleVentas
                .Include(d => d.Producto)
                .Where(d => d.VentaId == id)
                .ToListAsync();

            using var ms = new MemoryStream();

            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            document.Add(new Paragraph("JOYERIA ORO")
                .SetFont(bold)
                .SetFontSize(18));

            document.Add(new Paragraph("Factura #" + venta.VentaId));
            document.Add(new Paragraph("Fecha: " + venta.Fecha));

            document.Add(new Paragraph(" "));

            var table = new Table(4);

            table.AddHeaderCell("Producto");
            table.AddHeaderCell("Cantidad");
            table.AddHeaderCell("Precio");
            table.AddHeaderCell("Total");

            foreach (var d in detalles)
            {
                table.AddCell(d.Producto.Nombre);
                table.AddCell(d.Cantidad.ToString());
                table.AddCell(d.Precio.ToString("C"));
                table.AddCell((d.Cantidad * d.Precio).ToString("C"));
            }

            document.Add(table);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("TOTAL: " + venta.Total.ToString("C"))
                .SetFont(bold));

            document.Close();

            return File(ms.ToArray(), "application/pdf", "Factura_" + venta.VentaId + ".pdf");
        }

        [HttpPost]
        public async Task<IActionResult> Create(int productoId, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(productoId);

            if (producto == null)
                return NotFound();

            decimal total = (decimal)(producto.Precio * cantidad);

            var venta = new Venta
            {
                Fecha = DateTime.Now,
                Total = total
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            var detalle = new DetalleVenta
            {
                VentaId = venta.VentaId,
                ProductoId = productoId,
                Cantidad = cantidad,
                Precio = (decimal)producto.Precio
            };

            _context.DetalleVentas.Add(detalle);

            // DESCONTAR STOCK
            producto.Stock -= cantidad;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // FACTURA PDF
        public async Task<IActionResult> FacturaPDF(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);

            if (venta == null)
                return NotFound();

            using var ms = new MemoryStream();

            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            var titulo = new Paragraph("JOYERIA ORO - FACTURA")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            document.Add(titulo);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Factura #: " + venta.VentaId));
            document.Add(new Paragraph("Fecha: " + venta.Fecha));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Total Pagado: $" + venta.Total).SetFont(boldFont));

            document.Close();

            return File(ms.ToArray(), "application/pdf", "FacturaVenta.pdf");
        }
    }
}