using Microsoft.AspNetCore.Mvc;
using Joyeriaoro.Models;
using Joyeriaoro.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Joyeriaoro.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // =============================
        // LOGIN (GET)
        // =============================
        public IActionResult Login()
        {
            return View();
        }

        // =============================
        // LOGIN (POST)
        // =============================
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = _usuarioService.ObtenerPorEmail(email);

            if (usuario != null &&
                !string.IsNullOrEmpty(usuario.PasswordHash) &&
                BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
                    new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                    new Claim(ClaimTypes.Role, usuario.Roles ?? "Usuario")
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        // =============================
        // LOGOUT
        // =============================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}