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
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Linijas")]
    public class LinijasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }

        public LinijasController(IUnitOfWork db)
        {
            Db = db;
        }
        // GET: api/Linijas
        [AllowAnonymous]
        public List<int> GetLinije()
        {
            List<Linija> linije = Db.Linija.GetAll().AsQueryable().ToList();
            List<int> BrojeviLinija = new List<int>();
            linije.ForEach(x => { BrojeviLinija.Add(x.RedniBroj); });

            return BrojeviLinija;
        }

        // GET: api/Linijas/5
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetLinija/{id}/{dan}")]
        public IHttpActionResult GetLinija(int id, string dan)
        {
            List<Linija> linije = Db.Linija.GetAll().AsQueryable().ToList();
            string retVal = String.Empty;

            linije.ForEach(x =>
            {
                if (x.RedniBroj.Equals(id))
                {
                    List<RedVoznje> redoviVoznje = x.RedoviVoznje.ToList();
                    redoviVoznje.ForEach(y =>
                    {
                        if (y.DanUNedelji.Equals(dan))
                        {
                            retVal = y.Polasci;
                        }
                    });
                }
            });

            if (!String.IsNullOrEmpty(retVal))
            {
                return Ok(retVal);
            } else
            {
                return NotFound();
            }
        }

        // PUT: api/Linijas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLinija(int id, Linija linija)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != linija.Id)
            {
                return BadRequest();
            }

            db.Entry(linija).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinijaExists(id))
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

        // POST: api/Linijas
        [ResponseType(typeof(Linija))]
        public IHttpActionResult PostLinija(Linija linija)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Linije.Add(linija);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = linija.Id }, linija);
        }

        // DELETE: api/Linijas/5
        [ResponseType(typeof(Linija))]
        public IHttpActionResult DeleteLinija(int id)
        {
            Linija linija = db.Linije.Find(id);
            if (linija == null)
            {
                return NotFound();
            }

            db.Linije.Remove(linija);
            db.SaveChanges();

            return Ok(linija);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LinijaExists(int id)
        {
            return db.Linije.Count(e => e.Id == id) > 0;
        }
    }
}