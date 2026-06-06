using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Joyeriaoro.Data;
using Joyeriaoro.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Joyeriaoro.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTA DE PRODUCTOS + BUSCADOR
        public async Task<IActionResult> Index(string buscar)
        {
            var productos = from p in _context.Productos
                            select p;

            if (!string.IsNullOrEmpty(buscar))
            {
                productos = productos.Where(p => p.Nombre.Contains(buscar));
            }

            return View(await productos.ToListAsync());
        }

        // ABRIR FORMULARIO CREAR
        public IActionResult Create()
        {
            return View();
        }

        // GUARDAR PRODUCTO + IMAGEN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                if (archivo != null && archivo.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes/productos");

                    if (!Directory.Exists(carpeta))
                    {
                        Directory.CreateDirectory(carpeta);
                    }

                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    producto.ImagenUrl = nombreArchivo;
                }

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // INVENTARIO
        public IActionResult Inventario()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        // RUTA PARA REPORTE
        public IActionResult ReporteInventario()
        {
            return RedirectToAction("ReporteInventarioPDF");
        }

        // PDF INVENTARIO
        public async Task<IActionResult> ReporteInventarioPDF()
        {
            var productos = await _context.Productos.ToListAsync();

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            var titulo = new Paragraph("Reporte de Inventario - Joyeria Oro")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20);

            document.Add(titulo);

            var table = new Table(5).UseAllAvailableWidth();

            table.AddHeaderCell("ID");
            table.AddHeaderCell("Nombre");
            table.AddHeaderCell("Descripcion");
            table.AddHeaderCell("Precio");
            table.AddHeaderCell("Stock");

            foreach (var p in productos)
            {
                table.AddCell(p.Id.ToString());
                table.AddCell(p.Nombre ?? "Sin nombre");
                table.AddCell(p.Descripcion ?? "Sin descripcion");
                table.AddCell((p.Precio ?? 0).ToString("C"));
                table.AddCell((p.Stock ?? 0).ToString());
            }

            document.Add(table);

            decimal total = productos.Sum(p => (p.Precio ?? 0m) * (p.Stock ?? 0));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Valor total del inventario: $" + total)
                .SetFont(boldFont));

            document.Close();

            return File(ms.ToArray(), "application/pdf", "ReporteInventario.pdf");
        }
    }
}