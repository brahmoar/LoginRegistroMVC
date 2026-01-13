using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LoginRegistroMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace LoginRegistroMVC.Controllers
{
    public class LoginController : Controller
    {
        private static List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { NombreUsuario = "ADMIN", Contrasena = "ADMIN" },
            new Usuario { NombreUsuario = "usuario1", Contrasena = "pass123" },
            new Usuario { NombreUsuario = "test", Contrasena = "test123" }
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ValidarUsuario([FromBody] Usuario modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo.NombreUsuario))
            {
                return Json(new { success = false, message = "Usuario no puede encontrarse vacío" });
            }

            if (string.IsNullOrWhiteSpace(modelo.Contrasena))
            {
                return Json(new { success = false, message = "Contraseña no puede encontrarse vacía" });
            }

            var usuarioValido = usuarios.FirstOrDefault(u =>
                u.NombreUsuario == modelo.NombreUsuario &&
                u.Contrasena == modelo.Contrasena);

            if (usuarioValido != null)
            {
                HttpContext.Session.SetString("UsuarioActivo", usuarioValido.NombreUsuario);
                return Json(new { success = true, message = "Login exitoso" });
            }
            else
            {
                return Json(new { success = false, message = "Usuario no se encuentra en el sistema" });
            }
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}