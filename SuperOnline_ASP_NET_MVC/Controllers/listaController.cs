using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperOnline_ASP_NET_MVC.Models;
using System.Web.Script.Serialization;

namespace SuperOnline_ASP_NET_MVC.Controllers
{
    public class listaController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        //
        // GET: /lista/

        public ActionResult Index()
        {
            ViewBag.superMercados = db.Super_mercados.ToList();
            ViewBag.gondolas = db.Gondolas.ToList();
            ViewBag.categorias = db.Categorias.ToList();
            ViewBag.tipos = db.Tipos.ToList();

            ViewBag.lista = (string[])Session["lista"];

            if (Session["lista"] != null)
            {
                ViewBag.listaContiene = true;
                var listas = db.Listas.Include(l => l.Usuario);
                return View(listas.ToList());
            }
            else
            {
                ViewBag.listaContiene = false;
                return View();
            } 
        }

        public ActionResult CreateWithJson(List<String> values)
        {
            Lista lista = new Lista();
            lista.nombre = values[0];
            lista.usuario_id = 1;
            lista.fecha = DateTime.Today;

            db.Listas.Add(lista);

            Contenido_Listas contenido;
            for(var i = 0; i < values.Count; i++) {
                contenido = new Contenido_Listas();
                contenido.lista_id = 1;
                contenido.tipo = values[i];
                db.Contenido_Listas.Add(contenido);
            }
            db.SaveChanges();
            return View();
        }

        public ActionResult utilizarLista(List<String> values)
        {
            //values[0] -> nombre lista
            //values[>=1] -> contenido de la lista 
            
            //usuario logeado
            if (Session["correoUsuario"] != null)
            {
                Lista lista = new Lista();
                guardarListaEnSession(values.ToArray());
                GuardarLista(values);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //usuario no logeado
                guardarListaEnSession(values.ToArray());
                return Json(true, JsonRequestBehavior.AllowGet); 
            }
        }

        public void guardarListaEnSession(string[] lista) {
            //Usuario no logeado
            Session["lista"] = lista;
        }

        public ActionResult GuardarLista(List<String> values)
        {
            //values[0] -> nombre lista
            //values[>=1] -> contenido de la lista 

            //usuario logeado
            if (Session["correoUsuario"] != null)
            {
                var rows = db.Listas.ToList();

                var nextId = 0;
                if (rows.Count > 0)
                {
                    var lastRow = rows[rows.Count - 1];
                    var lastId = lastRow.lista_id;
                    nextId = lastId + 1;
                }
                else
                {
                    nextId++;
                }

                Lista lista = new Lista();
                lista.usuario_id = Int32.Parse(Session["idUsuario"].ToString());
                lista.nombre = values[0];
                lista.fecha = DateTime.Today;
                lista.lista_id = nextId;

                db.Listas.Add(lista);
                db.SaveChanges();

                Contenido_Listas contenido;

                for (var i = 1; i < values.Count; i++)
                {
                    contenido = new Contenido_Listas();
                    contenido.lista_id = nextId;
                    contenido.tipo = values[i];
                    db.Contenido_Listas.Add(contenido);
                }
                db.SaveChanges();
                guardarListaEnSession(values.ToArray());
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //usuario no logeado
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult miLista() {
            ViewBag.superMercados = db.Super_mercados.ToList();

            if (Session["correoUsuario"] != null)
            {
                var idUsuario = Int32.Parse(Session["idUsuario"].ToString());
                var usuarioTieneLista = (from l in db.Listas where l.usuario_id == idUsuario select l).Count();

                if (usuarioTieneLista > 0)
                {
                    var l = db.Listas.Where(s => s.usuario_id == idUsuario).OrderByDescending(a => a.lista_id).First();
                    if (l != null)
                    {
                        var Id = l.lista_id;

                        var contenido = (from c in db.Contenido_Listas
                                         where c.lista_id == Id
                                         select c);
                        ViewBag.listaExiste = true;
                        ViewBag.nombreLista = l.nombre;
                        return View(db.Contenido_Listas.Where(s => s.lista_id == Id).ToList());
                    }
                    else if (Session["lista"] != null)
                    {
                        ViewBag.listaExiste = false;
                        return View();
                    }
                }
                else if (Session["lista"] != null)
                {
                    string[] sessionLista = (string[])Session["lista"];

                    ViewBag.listaExiste = true;
                    ViewBag.nombreLista = sessionLista[0];
                    return View(sessionLista);
                }
                else {
                    ViewBag.listaExiste = false;
                    return View();
                }
            }
            else if (Session["lista"] != null) {

                string[] sessionLista = (string[])Session["lista"];
                List<Contenido_Listas> lista = new List<Contenido_Listas>();
                Contenido_Listas contenido;
                
                int c = 1;
                for (var i = 0; i < sessionLista.Length; i++) {
                    if (c != 1)
                    {
                        contenido = new Contenido_Listas();
                        contenido.tipo = sessionLista[i];
                        lista.Add(contenido);
                    }
                    c++;
                }

                ViewBag.listaExiste = true;
                ViewBag.nombreLista = sessionLista[0];
                return View(lista);
            }
            ViewBag.listaExiste = false;
            return View();
        }
    }
}