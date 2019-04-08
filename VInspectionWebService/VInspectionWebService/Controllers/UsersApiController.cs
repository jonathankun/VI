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
    public class UsersApiController : ApiController
    {
        private VInspectionEntities db = new VInspectionEntities();

        // GET: api/UsersApi
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/UsersApi/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [Route("api/UsersApi/GetUserByAccount/{account}")]
        [ResponseType(typeof(List<User>))]
        public IHttpActionResult GetUserByAccount(string account)
        {
            var user = db.Users.FirstOrDefault(x => x.Cuenta == account);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("api/UsersApi/GetUsersName/")]
        [ResponseType(typeof(List<User>))]
        public IHttpActionResult GetUsersName()
        {
            var usersByName = db.Users.Select(x => new { x.IdUsuario, x.Nombre, x.Area, x.Perfil });

            List<User> users = new List<User>();

            foreach (var item in usersByName)
            {
                User user = new User { IdUsuario = item.IdUsuario, Nombre = item.Nombre, Area = item.Area, Perfil = item.Perfil };
                users.Add(user);
            }

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/UsersApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.IdUsuario)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/UsersApi
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.IdUsuario }, user);
        }

        // DELETE: api/UsersApi/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.IdUsuario == id) > 0;
        }
    }
}