using SuperOnline_ASP_NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperOnline_ASP_NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        public ActionResult Index()
        {
            ViewBag.superMercados = db.Super_mercados.ToList();
            ViewBag.categorias = db.Categorias.ToList();
            ViewBag.tipos = db.Tipos.ToList();
   
            return View();
        }
    }
}
