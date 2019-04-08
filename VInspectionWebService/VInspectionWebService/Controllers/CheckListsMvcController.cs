using Newtonsoft.Json;
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
    public class CheckListsMvcController : Controller
    {
        private VInspectionEntities db = new VInspectionEntities();

        // GET: CheckListsMvc
        public ActionResult Index()
        {
            return View(db.CheckLists.ToList());
        }

        // GET: CheckListsMvc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckList checkList = db.CheckLists.Find(id);
            if (checkList == null)
            {
                return HttpNotFound();
            }
            return View(checkList);
        }

        // GET: CheckListsMvc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckListsMvc/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPreUso,Fecha,Placa,Kilometraje,Mantto,Produccion,Destino,Conductor,SistemaDireccion,SistemaFrenos,Faros,LucesDireccionales,Asientos,Cinturones,Vidrios,LimpiaParabrisas,EspejoInterno,EspejoExterno,NivelAceite,NivelAgua,Combustible,Claxon,AlarmaRetorceso,RelojesIndicadores,Neumaticos,NeumaticoRepuesto,Extintor,ConosSeguridad,SogaArrastre,Botiquin,HerramientasLlaves,GataPalanca,Triangulo,Linterna,Cunas,Carroceria,Pertiga,Circulina,ComentariosAdicionales,Observacion1,Prioridad1,Observacion2,Prioridad2,Observacion3,Prioridad3,Observacion4,Prioridad4,CajaSoporte,Alcohol,Jabon,Algodon,Aposito,Bandas,Esparadrapo,Gasas1,Gasas2,Tijera,Venda,ComentariosBotiquin,EstadoSOAT,NuevaFechaSOAT,EstadRevTecnica,NuevaFechaRevTecnica,SOAT,RevTecnica,TarjetaPropiedad,CartillaSeguridad,CartillaERP,BanderaMantto,BanderaItems,BanderaComentarios,BanderaBotiquin,BanderaDocumentos,BanderaPrincipal,Buscador,Garitas")] CheckList checkList)
        {
            if (ModelState.IsValid)
            {
                db.CheckLists.Add(checkList);
                db.SaveChanges();

                Debug.Print("\n\n" + JsonConvert.SerializeObject(checkList) + "\n\n");

                SummariesGeneratorApiController generador = new SummariesGeneratorApiController();

                Debug.Print("Se inicia actualización de kilometraje del vehiculo");
                generador.Actualizar_KilometrajoVehiculo(checkList);
                Debug.Print("Se inicia creación de nuevo resumen de Pre-Uso");
                generador.Guardar_ResumenPreUsos(checkList);
                if (generador.Evaluar_ActualizacionesDocumentos(checkList))
                {
                    Debug.Print("Se inicia actualización de documentos");
                    generador.Guardar_ActualizacionesDocumentos(checkList);
                }

                return RedirectToAction("Index");
            }

            return View(checkList);
        }

        // GET: CheckListsMvc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckList checkList = db.CheckLists.Find(id);
            if (checkList == null)
            {
                return HttpNotFound();
            }
            return View(checkList);
        }

        // POST: CheckListsMvc/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPreUso,Fecha,Placa,Kilometraje,Mantto,Produccion,Destino,Conductor,SistemaDireccion,SistemaFrenos,Faros,LucesDireccionales,Asientos,Cinturones,Vidrios,LimpiaParabrisas,EspejoInterno,EspejoExterno,NivelAceite,NivelAgua,Combustible,Claxon,AlarmaRetorceso,RelojesIndicadores,Neumaticos,NeumaticoRepuesto,Extintor,ConosSeguridad,SogaArrastre,Botiquin,HerramientasLlaves,GataPalanca,Triangulo,Linterna,Cunas,Carroceria,Pertiga,Circulina,ComentariosAdicionales,Observacion1,Prioridad1,Observacion2,Prioridad2,Observacion3,Prioridad3,Observacion4,Prioridad4,CajaSoporte,Alcohol,Jabon,Algodon,Aposito,Bandas,Esparadrapo,Gasas1,Gasas2,Tijera,Venda,ComentariosBotiquin,EstadoSOAT,NuevaFechaSOAT,EstadRevTecnica,NuevaFechaRevTecnica,SOAT,RevTecnica,TarjetaPropiedad,CartillaSeguridad,CartillaERP,BanderaMantto,BanderaItems,BanderaComentarios,BanderaBotiquin,BanderaDocumentos,BanderaPrincipal,Buscador,Garitas")] CheckList checkList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checkList);
        }

        // GET: CheckListsMvc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckList checkList = db.CheckLists.Find(id);
            if (checkList == null)
            {
                return HttpNotFound();
            }
            return View(checkList);
        }

        // POST: CheckListsMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckList checkList = db.CheckLists.Find(id);
            db.CheckLists.Remove(checkList);
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
