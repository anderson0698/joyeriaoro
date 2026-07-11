using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Joyeriaoro.Filters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rol = context.HttpContext.Session.GetString("UsuarioRoles");

            if (rol == null || rol != _role)
            {
                context.Result = new RedirectToActionResult("Login", "Usuarios", null);
            }

            base.OnActionExecuting(context);
        }
    }
}