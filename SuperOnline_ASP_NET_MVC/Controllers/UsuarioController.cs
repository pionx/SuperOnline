using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnline_ASP_NET_MVC.Models;

namespace SuperOnline_ASP_NET_MVC.Controllers
{
    public class UsuarioController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        //
        // GET: /Usuario/

        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Create

        public ActionResult Registrarse()
        {
            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        public ActionResult Registrarse(Usuario usuario)
        {
            ViewBag.mensajeCedula = "";
            ViewBag.mensajeCorreo = "";

            bool cedulaExiste = db.Usuarios.Any(t => t.cedula == usuario.cedula);

            bool correoExiste = db.Usuarios.Any(t => t.correo == usuario.correo);

            if (cedulaExiste == false && correoExiste == false)
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    Session["idUsuario"] = usuario.usuario_id;
                    Session["correoUsuario"] = usuario.correo;
                    Session["claveUsuario"] = usuario.clave;
                    Session["nombreUsuario"] = usuario.nombre;
                    Session["apellidoUsuario"] = usuario.apellido;
                    Session["cedulaUsuario"] = usuario.cedula;

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                if (cedulaExiste) ViewBag.mensajeCedula = "Cédula ya está registrada en el sistema";
                if (correoExiste) ViewBag.mensajeCorreo = "Correo electrónico ya está registrado";
            }

            return View(usuario);
        }
        
        public ActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(InicioSesion sesion)
        {
            var existeCorreo = (from u in db.Usuarios
                                where u.correo == sesion.correo
                                select u).Count();

            if (existeCorreo > 0)
            {
                var credencialesCorrectas = (from u in db.Usuarios
                                            where u.correo == sesion.correo && u.clave == sesion.clave
                                            select u).Count();

                if (credencialesCorrectas > 0)
                {
                    var usuario = db.Usuarios.Where(s => s.correo == sesion.correo).Where(s => s.clave == sesion.clave).First();
                    Session["idUsuario"] = usuario.usuario_id;
                    Session["correoUsuario"] = usuario.correo;
                    Session["claveUsuario"] = usuario.clave;
                    Session["nombreUsuario"] = usuario.nombre;
                    Session["apellidoUsuario"] = usuario.apellido;
                    Session["cedulaUsuario"] = usuario.cedula;

                    return RedirectToAction("Index", "Home");
                }
                else {
                    ViewBag.errorCredenciales = "Datos erroneos";
                }
            }else
            {
                ViewBag.mensajeCorreo = "Correo no esτá registrado";
            }


            return View();
        }

        public ActionResult cerrarSesion()
        {
            Session["idUsuario"] = null;
            Session["correoUsuario"] = null;
            Session["claveUsuario"] = null;
            Session["nombreUsuario"] = null;
            Session["apellidoUsuario"] = null;
            Session["cedulaUsuario"] = null;

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Usuario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public IQueryable<Usuario> cedulaExiste { get; set; }
    }
}