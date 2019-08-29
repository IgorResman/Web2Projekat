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
    public class CenovniksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Cenovniks
        public IQueryable<Cenovnik> GetCenovnici()
        {
            return db.Cenovnici;
        }
        public IUnitOfWork Db { get; set; }

        public CenovniksController(IUnitOfWork db)
        {
            Db = db;
        }


        // PUT: api/Cenovniks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCenovnik(int id, Cenovnik cenovnik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cenovnik.IdCenovnik)
            {
                return BadRequest();
            }

            db.Entry(cenovnik).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CenovnikExists(id))
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

        // POST: api/Promen
        [AllowAnonymous]

        [Route("PostCenovnik")]
        public IHttpActionResult PostCenovnik(CenovnikBindingModel cenovnik)
        {
            Cenovnik cenNovi = new Cenovnik();
            cenNovi.VaziDo = DateTime.Parse(cenovnik.vaziDo);
            cenNovi.VaziOd = DateTime.Parse(cenovnik.vaziOd);
            cenNovi.IdCenovnik = cenovnik.id;
            cenNovi.CeneKarti = new List<CenaKarte>();

            CenaKarte SD = new CenaKarte()
            {
                Cena = cenovnik.dnevna - (cenovnik.dnevna * cenovnik.popustStudent / 100),
                TipKupca = "Student",
                TipKarte = "Dnevna",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(SD);
            Db.CenaKarte.Add(SD);

            CenaKarte SV = new CenaKarte()
            {
                Cena = cenovnik.vremenska - (cenovnik.vremenska * cenovnik.popustStudent / 100),
                TipKupca = "Student",
                TipKarte = "Vremenska",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(SV);
            Db.CenaKarte.Add(SV);

            CenaKarte SM = new CenaKarte()
            {
                Cena = cenovnik.mesecna - (cenovnik.mesecna * cenovnik.popustStudent / 100),
                TipKupca = "Student",
                TipKarte = "Mesecna",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(SM);
            Db.CenaKarte.Add(SM);

            CenaKarte SG = new CenaKarte()
            {
                Cena = cenovnik.godisnja - (cenovnik.godisnja * cenovnik.popustStudent / 100),
                TipKupca = "Student",
                TipKarte = "Godisnja",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(SG);
            Db.CenaKarte.Add(SG);

            CenaKarte PD = new CenaKarte()
            {
                Cena = cenovnik.dnevna - (cenovnik.dnevna * cenovnik.popustPenzija / 100),
                TipKupca = "Penzioner",
                TipKarte = "Dnevna",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(PD);
            Db.CenaKarte.Add(PD);

            CenaKarte PV = new CenaKarte()
            {
                Cena = cenovnik.vremenska - (cenovnik.dnevna * cenovnik.popustPenzija / 100),
                TipKupca = "Penzioner",
                TipKarte = "Vremenska",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(PV);
            Db.CenaKarte.Add(PV);

            CenaKarte PM = new CenaKarte()
            {
                Cena = cenovnik.mesecna - (cenovnik.dnevna * cenovnik.popustPenzija / 100),
                TipKupca = "Penzioner",
                TipKarte = "Mesecna",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(PM);
            Db.CenaKarte.Add(PM);

            CenaKarte PG = new CenaKarte()
            {
                Cena = cenovnik.godisnja - (cenovnik.dnevna * cenovnik.popustPenzija / 100),
                TipKupca = "Penzioner",
                TipKarte = "Godisnja",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(PG);
            Db.CenaKarte.Add(PG);

            CenaKarte OD = new CenaKarte()
            {
                Cena = cenovnik.dnevna,
                TipKupca = "Obican",
                TipKarte = "Dnevna",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(OD);
            Db.CenaKarte.Add(OD);

            CenaKarte OV = new CenaKarte()
            {
                Cena = cenovnik.vremenska,
                TipKupca = "Obican",
                TipKarte = "Vremenska",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(OV);
            Db.CenaKarte.Add(OV);

            CenaKarte OM = new CenaKarte()
            {
                Cena = cenovnik.mesecna,
                TipKupca = "Obican",
                TipKarte = "Mesecna",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(OM);
            Db.CenaKarte.Add(OM);

            CenaKarte OG = new CenaKarte()
            {
                Cena = cenovnik.godisnja,
                TipKupca = "Obican",
                TipKarte = "Godisnja",
                CenovnikId = cenNovi.IdCenovnik
            };
            cenNovi.CeneKarti.Add(OG);
            Db.CenaKarte.Add(OG);

            Db.Cenovnik.Add((cenNovi));

            Db.Complete();

            return Ok();
        }

        // DELETE: api/Cenovniks/5
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteCenovnik(int id)
        {
            Cenovnik cenovnik = db.Cenovnici.Find(id);
            if (cenovnik == null)
            {
                return NotFound();
            }

            db.Cenovnici.Remove(cenovnik);
            db.SaveChanges();

            return Ok("Obrisan cenovnik");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CenovnikExists(int id)
        {
            return db.Cenovnici.Count(e => e.IdCenovnik == id) > 0;
        }
    }
}