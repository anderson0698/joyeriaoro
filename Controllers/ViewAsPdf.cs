using Joyeriaoro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Joyeriaoro.Controllers
{
    internal class ViewAsPdf : IActionResult
    {
        private string v;
        private List<Producto> productos;

        public ViewAsPdf(string v, List<Producto> productos)
        {
            this.v = v;
            this.productos = productos;
        }

        public string FileName { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}