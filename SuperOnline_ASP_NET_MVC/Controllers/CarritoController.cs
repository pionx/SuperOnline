using Newtonsoft.Json;
using SuperOnline_ASP_NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SuperOnline_ASP_NET_MVC.Controllers
{
    public class CarritoController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();
        
        [HttpPost]
        public ActionResult guardarCarritoJumboEnSession(List<String> articulos)
        {
            Session["carritoJumbo"] = articulos.ToArray();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult guardarCarritoLaSirenaEnSession(List<String> articulos)
        {
            Session["carritoLaSirena"] = articulos.ToArray();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult obtenerCarritoJumboDeSession() {

            if (Session["carritoJumbo"] != null)
            {
                string[] carrito = (string[])Session["carritoJumbo"];
                return Json(carrito, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult obtenerCarritoLaSirenaDeSession()
        {

            if (Session["carritoLaSirena"] != null)
            {
                string[] carrito = (string[])Session["carritoLaSirena"];
                return Json(carrito, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult RevisarCarrito(String superMercado)
        {
            var superMercadoSinEspacio = superMercado.Replace(" ", "");
            if (Session["carrito" + superMercadoSinEspacio] != null)
            {
                //var otrosSuperMercados = db.Super_mercados.Where(s=> s.nombre != superMercado);
                var otrosSuperMercados = (from s in db.Super_mercados where s.nombre != superMercadoSinEspacio select s).ToList();
                JavaScriptSerializer js = new JavaScriptSerializer();

                string[] sessionCarrito = (string[])Session["carrito" + superMercadoSinEspacio];
                string carritoJson = String.Join("", sessionCarrito);

                ArticuloCarrito[] carrito = js.Deserialize<ArticuloCarrito[]>(carritoJson);

                float totalSuper1 = 0;
                foreach (var item in carrito)
                {
                    totalSuper1 += item.total;
                }
                ViewBag.totalSuper1 = totalSuper1;

                //segundo super Mercado
                ArticuloCarrito[] otroCarrito = otrosCarritos(carrito, otrosSuperMercados[0].nombre);

                float totalSuper2 = 0;
                foreach (var item in otroCarrito)
                {
                    totalSuper2 += item.total;
                }
                ViewBag.totalSuper2 = totalSuper2;

                ViewBag.superMercado1 = superMercadoSinEspacio;
                ViewBag.superMercado2 = otrosSuperMercados[0].nombre;
                ViewBag.otroSuperMercado = otroCarrito;
                return View(carrito);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        ArticuloCarrito[] otrosCarritos(ArticuloCarrito[] carritoGuia, string superMercado)
        {
            ArticuloCarrito[] carrito = new ArticuloCarrito[carritoGuia.Length];

            for (var i = 0; i < carritoGuia.Length; i++) {
                ArticuloCarrito articulo = new ArticuloCarrito();
                string nombre = carritoGuia[i].nombre;
                var fila = db.Productos.Where(p => p.nombre == nombre).Where(p => p.Super_mercado == superMercado).First();
                articulo.nombre = fila.nombre;
                articulo.cant = carritoGuia[i].cant;
                articulo.precio = (float) fila.precio;
                articulo.total = Convert.ToInt32(articulo.cant) * articulo.precio;

                carrito[i] = articulo;
            }
            return carrito;
        }
    }
}
