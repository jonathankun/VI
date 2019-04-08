using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VInspectionWebService.Models;

namespace VInspectionWebService.Controllers
{
    public class CheckListSummariesApiController : ApiController
    {
        private VInspectionEntities db = new VInspectionEntities();

        // GET: api/CheckListSummariesApi
        public IQueryable<CheckListSummary> GetCheckListSummaries()
        {
            return db.CheckListSummaries;
        }

        // GET: api/CheckListSummariesApi/5
        [ResponseType(typeof(CheckListSummary))]
        public IHttpActionResult GetCheckListSummary(int id)
        {
            CheckListSummary checkListSummary = db.CheckListSummaries.Find(id);
            if (checkListSummary == null)
            {
                return NotFound();
            }

            return Ok(checkListSummary);
        }


        [Route("api/CheckListSummariesApi/GetCheckListSummariesByBrowser/{browser}")]
        [ResponseType(typeof(List<CheckListSummary>))]
        public IHttpActionResult GetCheckListSummariesByBrowser(string browser)
        {
            List<CheckListSummary> Summaries = db.CheckListSummaries.Where(x => x.Buscador.Contains(browser)).ToList();
            if (Summaries == null)
            {
                return NotFound();
            }

            return Ok(Summaries);
        }

        // PUT: api/CheckListSummariesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCheckListSummary(int id, CheckListSummary checkListSummary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != checkListSummary.IdResumen)
            {
                return BadRequest();
            }
            else
            {
                if (checkListSummary.Estado == 2 && checkListSummary.ComentariosVigilancia.Trim().Length > 0)
                {
                    SummariesGeneratorApiController generador = new SummariesGeneratorApiController();

                    Debug.Print("Se notifica a encargado de Security");
                    generador.Enviar_CorreoNotificacion(checkListSummary, SummariesGeneratorApiController.MOTIVO_RECHAZAR);
                }
            }

            db.Entry(checkListSummary).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckListSummaryExists(id))
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

        // POST: api/CheckListSummariesApi
        [ResponseType(typeof(CheckListSummary))]
        public IHttpActionResult PostCheckListSummary(CheckListSummary checkListSummary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CheckListSummaries.Add(checkListSummary);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = checkListSummary.IdResumen }, checkListSummary);
        }

        // DELETE: api/CheckListSummariesApi/5
        [ResponseType(typeof(CheckListSummary))]
        public IHttpActionResult DeleteCheckListSummary(int id)
        {
            CheckListSummary checkListSummary = db.CheckListSummaries.Find(id);
            if (checkListSummary == null)
            {
                return NotFound();
            }

            db.CheckListSummaries.Remove(checkListSummary);
            db.SaveChanges();

            return Ok(checkListSummary);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CheckListSummaryExists(int id)
        {
            return db.CheckListSummaries.Count(e => e.IdResumen == id) > 0;
        }
    }
}