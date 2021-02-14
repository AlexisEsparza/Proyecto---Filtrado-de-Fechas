using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FiltroFechasVideo.Models;
using FiltroFechasVideo.Models.ViewModels;

namespace FiltroFechasVideo.Controllers
{
    public class ContratoController : Controller
    {
        private dbFiltroFechasVideoEntities db = new dbFiltroFechasVideoEntities();

        // GET: Contrato
        public ActionResult Index()
        {
            var tblContrato = db.tblContrato.Include(t => t.tblCliente);
            return View(tblContrato.ToList());
        }

        [HttpPost]
        public ActionResult Index(DateTime Desde, DateTime Hasta)
        {
            List<tblContrato> lista1 = db.tblContrato.OrderBy(m=>m.idCliente).ToList();
            List<tblContrato> lista2 = new List<tblContrato>();
            List<SumaContratos> lista3 = new List<SumaContratos>();
            int aux1 = 0;
            int aux2 = -1;
            int primeraFecha = 0;
            int segundaFecha = 0;

            foreach (var a in lista1)
            {
                primeraFecha = DateTime.Compare(Desde, a.fechaContrato);
                segundaFecha = DateTime.Compare(Hasta, a.fechaContrato);
                if ((primeraFecha == -1 || primeraFecha == 0) && (segundaFecha == 1 || segundaFecha == 0))
                {
                    lista2.Add(db.tblContrato.Where(p => p.idContrato == a.idContrato).First());
                }
            }

            foreach(var d in lista2)
            {
                if(d.idCliente==aux1)
                {
                    lista3[aux2].MontoContrato += d.montoContrato;
                }
                else
                {
                    lista3.Add(new SumaContratos()
                    {
                        Nombre = db.tblCliente.Where(p=>p.idCliente == d.idCliente).First().nombreCliente,
                        MontoContrato = d.montoContrato
                    });
                    
                    aux2++;
                    aux1 = d.idCliente;
                }
            }
            return View("ListaContratos",lista3);
        }
            

        public ActionResult ListaContratos()
        {
            return View();
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblContrato tblContrato = db.tblContrato.Find(id);
            if (tblContrato == null)
            {
                return HttpNotFound();
            }
            return View(tblContrato);
        }

        // GET: Contrato/Create
        public ActionResult Create()
        {
            ViewBag.idCliente = new SelectList(db.tblCliente, "idCliente", "nombreCliente");
            return View();
        }

        // POST: Contrato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContrato,idCliente,montoContrato,fechaContrato")] tblContrato tblContrato)
        {
            if (ModelState.IsValid)
            {
                db.tblContrato.Add(tblContrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.tblCliente, "idCliente", "nombreCliente", tblContrato.idCliente);
            return View(tblContrato);
        }

        // GET: Contrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblContrato tblContrato = db.tblContrato.Find(id);
            if (tblContrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.tblCliente, "idCliente", "nombreCliente", tblContrato.idCliente);
            return View(tblContrato);
        }

        // POST: Contrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idContrato,idCliente,montoContrato,fechaContrato")] tblContrato tblContrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblContrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.tblCliente, "idCliente", "nombreCliente", tblContrato.idCliente);
            return View(tblContrato);
        }

        // GET: Contrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblContrato tblContrato = db.tblContrato.Find(id);
            if (tblContrato == null)
            {
                return HttpNotFound();
            }
            return View(tblContrato);
        }

        // POST: Contrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblContrato tblContrato = db.tblContrato.Find(id);
            db.tblContrato.Remove(tblContrato);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//var array = db.buscar(From, To);