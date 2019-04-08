using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VInspectionWebService.Models;

namespace VInspectionWebService.Controllers
{
    public class CheckListsApiController : ApiController
    {
        private VInspectionEntities db = new VInspectionEntities();

        // GET: api/CheckListsApi
        public IQueryable<CheckList> GetCheckLists()
        {
            return db.CheckLists;
        }

        // GET: api/CheckListsApi/5
        [ResponseType(typeof(CheckList))]
        public IHttpActionResult GetCheckList(int id)
        {
            CheckList checkList = db.CheckLists.Find(id);
            if (checkList == null)
            {
                return NotFound();
            }

            return Ok(checkList);
        }

        // PUT: api/CheckListsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCheckList(int id, CheckList checkList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != checkList.IdPreUso)
            {
                return BadRequest();
            }

            db.Entry(checkList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CheckListsApi
        [ResponseType(typeof(CheckList))]
        public IHttpActionResult PostCheckList(CheckList checkList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            return CreatedAtRoute("DefaultApi", new { id = checkList.IdPreUso }, checkList);
        }

        // DELETE: api/CheckListsApi/5
        [ResponseType(typeof(CheckList))]
        public IHttpActionResult DeleteCheckList(int id)
        {
            CheckList checkList = db.CheckLists.Find(id);
            if (checkList == null)
            {
                return NotFound();
            }

            db.CheckLists.Remove(checkList);
            db.SaveChanges();

            return Ok(checkList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CheckListExists(int id)
        {
            return db.CheckLists.Count(e => e.IdPreUso == id) > 0;
        }
    }
}