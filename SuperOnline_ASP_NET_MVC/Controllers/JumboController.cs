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
    public class JumboController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();
              
        public ActionResult Producto(string tipo = null)
        {
            ViewBag.superMercados = db.Super_mercados.ToList();
            ViewBag.gondolas = db.Gondolas.ToList();
            ViewBag.categorias = db.Categorias.ToList();
            ViewBag.tipos = db.Tipos.ToList();
            ViewBag.lista = (string[])Session["lista"];
             
            if (Session["lista"] != null) { 
              ViewBag.listaContiene = true;  
            }else{
                ViewBag.listaContiene = false; 
            }

            ViewBag.carritoJumbo = (string[]) Session["carritoJumbo"];

            
            return View(db.Productos.Where(s => s.Super_mercado == "Jumbo").Where(s => s.tipo == tipo).ToList());
        }

        public ActionResult Buscar(List<String> buscar)
        {
            var patron = buscar[0];
            List<Producto> productos = (from p in db.Productos where p.nombre.Contains(patron) && p.Super_mercado == "jumbo" select p).ToList();
            
            ViewBag.superMercados = db.Super_mercados.ToList();
            ViewBag.gondolas = db.Gondolas.ToList();
            ViewBag.categorias = db.Categorias.ToList();
            ViewBag.tipos = db.Tipos.ToList();
            ViewBag.lista = (string[])Session["lista"];
             
            if (Session["lista"] != null) { 
                ViewBag.listaContiene = true;  
            }else{
                ViewBag.listaContiene = false; 
            }

            ViewBag.carritoJumbo = (string[]) Session["carritoJumbo"];

            if (productos.Count > 0)
            {
                ViewBag.resultado = true;
                return View(productos);
            }
            else {
                ViewBag.resultado = false;
                return View(productos);            
            }
        }
    }
}
