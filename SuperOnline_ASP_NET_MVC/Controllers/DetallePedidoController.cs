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
    public class DetallePedidoController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        //
        // GET: /DetallePedido/

        public ActionResult Index()
        {
            var detalle_pedido = db.Detalle_pedido.Include(d => d.Producto).Include(d => d.Pedido);
            return View(detalle_pedido.ToList());
        }

        //
        // GET: /DetallePedido/Details/5

        public ActionResult Details(int id = 0)
        {
            Detalle_pedido detalle_pedido = db.Detalle_pedido.Find(id);
            if (detalle_pedido == null)
            {
                return HttpNotFound();
            }
            return View(detalle_pedido);
        }

        //
        // GET: /DetallePedido/Create

        public ActionResult Create()
        {
            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre");
            ViewBag.pedido_id = new SelectList(db.Pedidos, "pedido_id", "pedido_id");
            return View();
        }

        //
        // POST: /DetallePedido/Create

        [HttpPost]
        public ActionResult Create(Detalle_pedido detalle_pedido)
        {
            if (ModelState.IsValid)
            {
                db.Detalle_pedido.Add(detalle_pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre", detalle_pedido.producto_id);
            ViewBag.pedido_id = new SelectList(db.Pedidos, "pedido_id", "estado", detalle_pedido.pedido_id);
            return View(detalle_pedido);
        }

        //
        // GET: /DetallePedido/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Detalle_pedido detalle_pedido = db.Detalle_pedido.Find(id);
            if (detalle_pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre", detalle_pedido.producto_id);
            ViewBag.pedido_id = new SelectList(db.Pedidos, "pedido_id", "estado", detalle_pedido.pedido_id);
            return View(detalle_pedido);
        }

        //
        // POST: /DetallePedido/Edit/5

        [HttpPost]
        public ActionResult Edit(Detalle_pedido detalle_pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalle_pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.producto_id = new SelectList(db.Productos, "producto_id", "nombre", detalle_pedido.producto_id);
            ViewBag.pedido_id = new SelectList(db.Pedidos, "pedido_id", "estado", detalle_pedido.pedido_id);
            return View(detalle_pedido);
        }

        //
        // GET: /DetallePedido/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Detalle_pedido detalle_pedido = db.Detalle_pedido.Find(id);
            if (detalle_pedido == null)
            {
                return HttpNotFound();
            }
            return View(detalle_pedido);
        }

        //
        // POST: /DetallePedido/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Detalle_pedido detalle_pedido = db.Detalle_pedido.Find(id);
            db.Detalle_pedido.Remove(detalle_pedido);
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