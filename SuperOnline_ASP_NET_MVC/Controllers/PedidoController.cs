using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnline_ASP_NET_MVC.Models;
using System.Web.Script.Serialization;
using SuperOnline_ASP_NET_MVC.Models;

namespace SuperOnline_ASP_NET_MVC.Controllers
{
    public class PedidoController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        //
        // GET: /Pedido/

        public ActionResult Index()
        {
            if (Session["idUsuario"] != null)
            {
                int idUsuario = (int)Session["idUsuario"];
                //var hayPedidos = (from p in db.Pedidos select p).Count();
                var pedidos = db.Pedidos.Where(p => p.usuario_id == idUsuario);

                if (pedidos.Count() > 0)
                {
                    ViewBag.logeado = true;
                    ViewBag.hayPedidos = true;
                    return View(pedidos.ToList());
                }
                ViewBag.hayPedidos = false;
                return View();
            }
            else
            {
                ViewBag.hayPedidos = false;
                ViewBag.logeado = false;
                return View();
            }
        }

        //
        // GET: /Pedido/Create
        public ActionResult Create(string superMercado)
        {

            if (Session["carrito" + superMercado] != null && Session["correoUsuario"] != null)
            {
                //Obtener articulos de carrito 
                JavaScriptSerializer js = new JavaScriptSerializer();

                string[] sessionCarrito = (string[])Session["carrito" + superMercado];
                string carritoJson = String.Join("", sessionCarrito);

                ArticuloCarrito[] carrito = js.Deserialize<ArticuloCarrito[]>(carritoJson);

                float total = 0;
                foreach (var item in carrito)
                {
                    total += item.total;
                }

                //obtener proximo id de pedido
                var rows = db.Detalle_pedido.ToList();

                var nextId = 0;
                if (rows.Count > 0)
                {
                    var lastRow = rows[rows.Count - 1];
                    var lastId = lastRow.pedido_id;
                    nextId = lastId + 1;
                }
                else
                {
                    nextId++;
                }

                Pedido pedido = new Pedido();
                pedido.pedido_id = nextId;
                pedido.estado = "Pendiente";
                pedido.total_precio = Convert.ToDecimal(total);
                pedido.fecha = DateTime.Today;
                pedido.usuario_id = (int)Session["idUsuario"];
                pedido.super_mercado = superMercado;
                db.Pedidos.Add(pedido);

                db.SaveChanges();

                Detalle_pedido detalle;

                for (var i = 1; i < carrito.Length; i++)
                {
                    detalle = new Detalle_pedido();
                    detalle.pedido_id = nextId;
                    detalle.cantidad_producto = Convert.ToInt32(carrito[i].cant);
                    detalle.precio = Convert.ToDecimal(carrito[i].precio);
                    detalle.sub_total = Convert.ToDecimal(carrito[i].total);

                    var nombreProducto = carrito[i].nombre;
                    var productoId = (from p in db.Productos where p.nombre == nombreProducto select p.producto_id).First();
                    detalle.producto_id = productoId;
                    db.Detalle_pedido.Add(detalle);

                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else 
            {
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Pedido/Create

        [HttpPost]
        public ActionResult Create(Pedido pedido)
        {
            if (ModelState.IsValid)
            {   
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.super_mercado = new SelectList(db.Super_mercados, "nombre", "nombre", pedido.super_mercado);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "cedula", pedido.usuario_id);
            return View(pedido);
        }

        //
        // GET: /Pedido/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        //
        // POST: /Pedido/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
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