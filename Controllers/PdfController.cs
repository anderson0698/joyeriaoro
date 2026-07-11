using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Joyeriaoro.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult Index()
        {
            // Creamos un MemoryStream para almacenar el PDF
            using var ms = new MemoryStream();

            // Creamos el escritor y el documento PDF
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Agregamos contenido
            document.Add(new Paragraph("Hola, este PDF fue generado con iText7!"));
            document.Add(new Paragraph("Puedes agregar tablas, imágenes y más."));

            // Cerramos el documento para finalizar el PDF
            document.Close();

            // Retornamos el PDF como un archivo descargable
            return File(ms.ToArray(), "application/pdf", "PruebaIText.pdf");
        }
    }
}