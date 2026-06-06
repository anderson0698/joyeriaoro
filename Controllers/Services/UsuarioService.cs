using Joyeriaoro.Data;
using Joyeriaoro.Models;
using System.Linq;

namespace Joyeriaoro.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Registrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.Email == email);
        }

        public Usuario? Login(string email, string password)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }
    }
}