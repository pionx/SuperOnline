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
    public class SuperController : Controller
    {
        private SuperOnlineEntities db = new SuperOnlineEntities();

        //
        // GET: /Super/

        public ActionResult Index()
        {
            return View(db.Super_mercados.ToList());
        }

        //
        // GET: /Super/Details/5

        public ActionResult Details(string id = null)
        {
            Super_mercados super_mercados = db.Super_mercados.Find(id);
            if (super_mercados == null)
            {
                return HttpNotFound();
            }
            return View(super_mercados);
        }

        //
        // GET: /Super/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Super/Create

        [HttpPost]
        public ActionResult Create(Super_mercados super_mercados)
        {
            if (ModelState.IsValid)
            {
                db.Super_mercados.Add(super_mercados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(super_mercados);
        }

        //
        // GET: /Super/Edit/5

        public ActionResult Edit(string id = null)
        {
            Super_mercados super_mercados = db.Super_mercados.Find(id);
            if (super_mercados == null)
            {
                return HttpNotFound();
            }
            return View(super_mercados);
        }

        //
        // POST: /Super/Edit/5

        [HttpPost]
        public ActionResult Edit(Super_mercados super_mercados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(super_mercados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(super_mercados);
        }

        //
        // GET: /Super/Delete/5

        public ActionResult Delete(string id = null)
        {
            Super_mercados super_mercados = db.Super_mercados.Find(id);
            if (super_mercados == null)
            {
                return HttpNotFound();
            }
            return View(super_mercados);
        }

        //
        // POST: /Super/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Super_mercados super_mercados = db.Super_mercados.Find(id);
            db.Super_mercados.Remove(super_mercados);
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