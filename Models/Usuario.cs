using System;
using System.ComponentModel.DataAnnotations;

namespace Joyeriaoro.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        public string Roles { get; set; } = "Vendedor";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}