using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VInspectionWebService.Models;

namespace VInspectionWebService.Controllers
{
    public class DocumentsApiController : ApiController
    {
        private VInspectionEntities db = new VInspectionEntities();

        // GET: api/DocumentsApi
        public IQueryable<Document> GetDocuments()
        {
            return db.Documents;
        }

        // GET: api/DocumentsApi/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult GetDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        [Route("api/DocumentsApi/GetDocumentsByPlate/{plate}")]
        [ResponseType(typeof(Document))]
        public IHttpActionResult GetDocumentsByPlate(string plate)
        {
            Document document = db.Documents.FirstOrDefault(x => x.Placa == plate);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/DocumentsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.IdDocumento)
            {
                return BadRequest();
            }

            db.Entry(document).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/DocumentsApi
        [ResponseType(typeof(Document))]
        public IHttpActionResult PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Documents.Add(document);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = document.IdDocumento }, document);
        }

        // DELETE: api/DocumentsApi/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult DeleteDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            db.Documents.Remove(document);
            db.SaveChanges();

            return Ok(document);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentExists(int id)
        {
            return db.Documents.Count(e => e.IdDocumento == id) > 0;
        }
    }
}