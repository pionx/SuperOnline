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
    public class comentarioController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        //
        // GET: /comentario/

        public ActionResult Index()
        {
            var comentarios = db.Comentarios.Include(c => c.Producto).Include(c => c.Usuario);
            return View(comentarios.ToList());
        }

        //
        // GET: /comentario/Details/5

        public ActionResult Details(int id = 0)
        {
            Comentario comentario = db.Comentarios.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        //
        // GET: /comentario/Create

        public ActionResult Create()
        {
            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre");
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "cedula");
            return View();
        }

        //
        // POST: /comentario/Create

        [HttpPost]
        public ActionResult Create(Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Comentarios.Add(comentario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre", comentario.producto_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "cedula", comentario.usuario_id);
            return View(comentario);
        }

        //
        // GET: /comentario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comentario comentario = db.Comentarios.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre", comentario.producto_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "cedula", comentario.usuario_id);
            return View(comentario);
        }

        //
        // POST: /comentario/Edit/5

        [HttpPost]
        public ActionResult Edit(Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre", comentario.producto_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "cedula", comentario.usuario_id);
            return View(comentario);
        }

        //
        // GET: /comentario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Comentario comentario = db.Comentarios.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        //
        // POST: /comentario/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comentario comentario = db.Comentarios.Find(id);
            db.Comentarios.Remove(comentario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}