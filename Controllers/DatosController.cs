using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LoginRegistroMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace LoginRegistroMVC.Controllers
{
    public class DatosController : Controller
    {
        private static List<Persona> personas = new List<Persona>();
        private static int contadorId = 1;

        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("UsuarioActivo");
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Usuario = usuario;
            return View();
        }

        [HttpPost]
        public JsonResult GuardarPersona([FromBody] Persona modelo)
        {
            var usuario = HttpContext.Session.GetString("UsuarioActivo");
            if (string.IsNullOrEmpty(usuario))
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            modelo.Id = contadorId++;
            personas.Add(modelo);

            return Json(new
            {
                success = true,
                persona = new
                {
                    id = modelo.Id,
                    nombreCompleto = modelo.NombreCompleto,
                    fechaNacimiento = modelo.FechaNacimiento.ToString("dd/MM/yyyy"),
                    edad = modelo.Edad,
                    correo = modelo.Correo
                }
            });
        }

        [HttpPost]
        public JsonResult EliminarPersona([FromBody] int id)
        {
            var persona = personas.FirstOrDefault(p => p.Id == id);
            if (persona != null)
            {
                personas.Remove(persona);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Persona no encontrada" });
        }

        [HttpGet]
        public JsonResult ObtenerPersonas()
        {
            var lista = personas.Select(p => new {
                id = p.Id,
                nombreCompleto = p.NombreCompleto,
                fechaNacimiento = p.FechaNacimiento.ToString("dd/MM/yyyy"),
                edad = p.Edad,
                correo = p.Correo
            });
            return Json(lista);
        }
    }
}