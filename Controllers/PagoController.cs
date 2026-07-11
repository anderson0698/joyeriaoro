using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Joyeriaoro.Models;
using Joyeriaoro.Data;   // 👈 IMPORTANTE

namespace Joyeriaoro.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Pagar(decimal precio)
        {
            var options = new SessionCreateOptions
            {
                CustomerEmail = User.Identity?.Name,

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(precio * 100),
                            Currency = "dop",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Producto Joyeria"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:7004/Pago/Exito?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7004/Home/Index"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult Exito(string session_id)
        {
            if (string.IsNullOrEmpty(session_id))
            {
                return RedirectToAction("Index", "Home");
            }

            var service = new SessionService();
            Session session = service.Get(session_id);

            var venta = new Venta
            {
                ProductoNombre = "Producto Joyeria",
                Precio = session.AmountTotal.HasValue ? session.AmountTotal.Value / 100 : 0,
                Fecha = DateTime.Now,

                UsuarioEmail = User.Identity?.Name
                                ?? session.CustomerEmail
                                ?? "cliente@anonimo.com",

                StripeSessionId = session.Id
            };

            _context.Ventas.Add(venta);
            _context.SaveChanges();

            return View();
        }
    }
}