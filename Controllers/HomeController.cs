using Joyeriaoro.Models;
using Joyeriaoro.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Joyeriaoro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================
        // PAGINA PRINCIPAL PUBLICA
        // =========================================
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(productos);
        }

        // =========================================
        // PRIVACIDAD
        // =========================================
        public IActionResult Privacy()
        {
            return View();
        }

        // =========================================
        // DASHBOARD ADMINISTRATIVO
        // =========================================
        [Authorize]
        public IActionResult Dashboard()
        {
            ViewBag.Nombre = User.Identity?.Name;
            ViewBag.Email = User.FindFirst(ClaimTypes.Email)?.Value;
            ViewBag.Rol = User.FindFirst(ClaimTypes.Role)?.Value;

            // =========================
            // INVENTARIO
            // =========================

            var totalProductos = _context.Productos.Count();

            var stockBajo = _context.Productos
                .Where(p => (p.Stock ?? 0) <= 5 &&
                            (p.Stock ?? 0) > 0)
                .Count();

            var agotados = _context.Productos
                .Where(p => (p.Stock ?? 0) == 0)
                .Count();

            var disponibles = _context.Productos
                .Where(p => (p.Stock ?? 0) > 5)
                .Count();

            var valorInventario = _context.Productos
                .Sum(p => (p.Precio ?? 0) *
                          (p.Stock ?? 0));

            var productosStockBajo = _context.Productos
                .Where(p => (p.Stock ?? 0) <= 5 &&
                            (p.Stock ?? 0) > 0)
                .ToList();

            // =========================
            // VENTAS POR MES
            // =========================

            var ventasMes = _context.Ventas
                .GroupBy(v => v.Fecha.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Sum(v => v.Total)
                })
                .OrderBy(x => x.Mes)
                .ToList();

            // =========================
            // TOP PRODUCTOS VENDIDOS
            // =========================

            var topProductos = _context.DetalleVentas
                .Include(d => d.Producto)
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new
                {
                    Nombre = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToList();

            // =========================
            // VIEWBAG
            // =========================

            ViewBag.TotalProductos = totalProductos;
            ViewBag.StockBajo = stockBajo;
            ViewBag.Agotados = agotados;
            ViewBag.Disponibles = disponibles;
            ViewBag.ValorInventario = valorInventario;
            ViewBag.ProductosStockBajo = productosStockBajo;
            ViewBag.VentasMes = ventasMes;
            ViewBag.TopProductos = topProductos;

            return View();
        }

        // =========================================
        // ERROR
        // =========================================
        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                            HttpContext.TraceIdentifier
            });
        }
    }
}