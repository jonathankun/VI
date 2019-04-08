using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VInspectionWebService.Models;

namespace VInspectionWebService.Controllers
{
    public class CheckListSummariesMvcController : Controller
    {
        private VInspectionEntities db = new VInspectionEntities();

        // GET: CheckListSummariesMvc
        public ActionResult Index()
        {
            return View(db.CheckListSummaries.ToList());
        }

        // GET: CheckListSummariesMvc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListSummary checkListSummary = db.CheckListSummaries.Find(id);
            if (checkListSummary == null)
            {
                return HttpNotFound();
            }
            return View(checkListSummary);
        }

        // GET: CheckListSummariesMvc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckListSummariesMvc/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdResumen,Vehiculo,Conductor,Fecha,Kilometraje,Produccion,Destino,MensajeMantto,Items1,Items2,Comentarios1,Comentarios2,Botiquin1,Botiquin2,Botiquin3,Seguridad1,Seguridad2,Seguridad3,Seguridad4,BanderaMantto,BanderaItems,BanderaComentarios,BanderaBotiquin,BanderaDocumentos,BanderaPrincipal,BanderaMensajes,ComentariosVigilancia,Buscador,Garitas,Estado")] CheckListSummary checkListSummary)
        {
            if (ModelState.IsValid)
            {
                db.CheckListSummaries.Add(checkListSummary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checkListSummary);
        }

        // GET: CheckListSummariesMvc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListSummary checkListSummary = db.CheckListSummaries.Find(id);
            if (checkListSummary == null)
            {
                return HttpNotFound();
            }
            return View(checkListSummary);
        }

        // POST: CheckListSummariesMvc/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdResumen,Vehiculo,Conductor,Fecha,Kilometraje,Produccion,Destino,MensajeMantto,Items1,Items2,Comentarios1,Comentarios2,Botiquin1,Botiquin2,Botiquin3,Seguridad1,Seguridad2,Seguridad3,Seguridad4,BanderaMantto,BanderaItems,BanderaComentarios,BanderaBotiquin,BanderaDocumentos,BanderaPrincipal,BanderaMensajes,ComentariosVigilancia,Buscador,Garitas,Estado")] CheckListSummary checkListSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkListSummary).State = EntityState.Modified;

                if (checkListSummary.Estado == 2 && checkListSummary.ComentariosVigilancia.Trim().Length > 0)
                {
                    SummariesGeneratorApiController generador = new SummariesGeneratorApiController();

                    Debug.Print("Se notifica a encargado de Security");
                    generador.Enviar_CorreoNotificacion(checkListSummary, SummariesGeneratorApiController.MOTIVO_RECHAZAR);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checkListSummary);
        }

        // GET: CheckListSummariesMvc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListSummary checkListSummary = db.CheckListSummaries.Find(id);
            if (checkListSummary == null)
            {
                return HttpNotFound();
            }
            return View(checkListSummary);
        }

        // POST: CheckListSummariesMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckListSummary checkListSummary = db.CheckListSummaries.Find(id);
            db.CheckListSummaries.Remove(checkListSummary);
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
