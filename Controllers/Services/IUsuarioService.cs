using Joyeriaoro.Models;

namespace Joyeriaoro.Services
{
    public interface IUsuarioService
    {
        void Registrar(Usuario usuario);

        Usuario? ObtenerPorEmail(string email);

        Usuario? Login(string email, string password);
    }
}