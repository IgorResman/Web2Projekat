using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
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
        public List<string> GetLinije()
        {
            List<Linija> linije = Db.Linija.GetAll().AsQueryable().ToList();
            List<string> BrojeviLinija = new List<string>();

            linije.ForEach(x =>
            {
                BrojeviLinija.Add(x.RedniBroj);
            });

            return BrojeviLinija;
        }

        // GET: api/Linijas/5
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetLinija/{id}/{dan}")]
        public IHttpActionResult GetLinija(string id, string dan)
        {
            List<Linija> linije = Db.Linija.GetAll().AsQueryable().ToList();

            string retVal = String.Empty;

            linije.ForEach(x =>
            {
                if (x.RedniBroj.Equals(id))
                {
                    foreach (RedVoznje red in x.RedoviVoznje)
                    {
                        if (red.DanUNedelji == dan)
                        {
                            retVal = red.Polasci;
                        }
                    }
                }
            });

            if (!String.IsNullOrEmpty(retVal))
            {
                return Ok(retVal);
            }

            return NotFound();
        }

        // GET: api/Linijas/5   
        //[AllowAnonymous]
        //[ResponseType(typeof(string))]
        //[Route("GetLinija/{id}/{dan}/{p}")]
        //public IHttpActionResult SeedDBWithLines(int id, string dan, string p)
        //{
        //    using (StreamReader r = new StreamReader(@"D:\WEB-FINAL\Jgsp-\sve.json"))
        //    {
        //        string json = "", linijaPodela = "";

        //        while ((json = r.ReadLine()) != null)
        //        {
        //            string[] linijaNiz;
        //            Stanica s = new Stanica();
        //            linijaPodela = json.Split('|')[0];
        //            linijaNiz = linijaPodela.Split(',', '[', ']');
        //            s.Adresa = json.Split('|')[3];
        //            string brojX = json.Split('|')[1];
        //            string brojY = json.Split('|')[2];
        //            s.X = double.Parse(brojX, CultureInfo.InvariantCulture);
        //            s.Y = double.Parse(brojY, CultureInfo.InvariantCulture);
        //            s.Naziv = s.Adresa = json.Split('|')[3];
        //            s.Linije = new List<Linija>();
        //            bool stanicaPostoji = false;
        //            List<Linija> stanLinije = new List<Linija>();

        //            foreach (var lin in linijaNiz)
        //            {
        //                if (lin != "" && lin != "    \"")
        //                {
        //                    Linija l = new Linija() { RedniBroj = lin };
        //                    List<Linija> sveLinije = Db.Linija.GetAll().ToList();
        //                    bool linijaPostoji = false;

        //                    foreach (var linija in sveLinije)
        //                    {
        //                        if (linija.RedniBroj == lin)
        //                        {
        //                            l = null;
        //                            l = linija;
        //                            linijaPostoji = true;
        //                            break;
        //                        }
        //                    }

        //                    if (linijaPostoji)
        //                    {
        //                        //s.Linije.Add(l);
        //                        //l.Stanice.Add(s);
        //                        //Db.Linija.Update(l);
        //                        //continue;
        //                        List<Stanica> sveStanice = Db.Stanica.GetAll().ToList();
        //                        foreach (var stanica in sveStanice)
        //                        {
        //                            if (stanica.Adresa == s.Adresa && stanica.X == s.X && stanica.Y == s.Y)
        //                            {
        //                                s = null;
        //                                s = stanica;
        //                                stanicaPostoji = true;
        //                                break;
        //                            }
        //                        }

        //                        if (stanicaPostoji)
        //                        {
        //                            l.Stanice.Add(s);
        //                            s.Linije.Add(l);
        //                            Db.Linija.Update(l);
        //                            Db.Stanica.Update(s);
        //                            //continue;
        //                        }
        //                        else
        //                        {
        //                            l.Stanice.Add(s);
        //                            s.Linije.Add(l);
        //                            Db.Linija.Update(l);
        //                            Db.Stanica.Add(s);
        //                        }
        //                        Db.Complete();
        //                    }
        //                    else
        //                    {
        //                        List<Stanica> sveStanice = Db.Stanica.GetAll().ToList();
        //                        foreach (var stanica in sveStanice)
        //                        {
        //                            if (stanica.Adresa == s.Adresa)
        //                            {
        //                                s = null;
        //                                s = stanica;
        //                                stanicaPostoji = true;
        //                                break;
        //                            }
        //                        }

        //                        if (stanicaPostoji)
        //                        {
        //                            s.Linije.Add(l);
        //                            Db.Stanica.Update(s);
        //                            //Db.Stanica.Update(s);
        //                            //continue;
        //                        }
        //                        else
        //                        {
        //                            s.Linije = new List<Linija>();
        //                            s.Linije.Add(l);
        //                            Db.Stanica.Add(s);
        //                        }
        //                        //s.Linije.Add(l);
        //                        l.Stanice = new List<Stanica>();
        //                        l.Stanice.Add(s);
        //                        Db.Linija.Add(l);
        //                        Db.Complete();
        //                    }
        //                }
        //            }
        //            //List<Stanica> sveStanice = Db.Stanica.GetAll().ToList();
        //            //foreach (var stanica in sveStanice)
        //            //{
        //            //    if (stanica.Adresa == s.Adresa)
        //            //    {
        //            //        s = stanica;
        //            //        stanicaPostoji = true;
        //            //        break;
        //            //    }
        //            //}

        //            //if (stanicaPostoji)
        //            //{
        //            //    //Db.Stanica.Update(s);
        //            //    continue;
        //            //}
        //            //else
        //            //{
        //            //    Db.Stanica.Add(s);
        //            //}
        //            //Db.Complete();
        //        }
        //    }

        //    return NotFound();
        //}

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
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetLinijaDodaj/{linija}")]
        public IHttpActionResult GetLinija(string linija)
        {
            if (ModelState.IsValid)
            {
                Linija lin = new Linija();
                lin.RedniBroj = linija;
                Db.Linija.Add(lin);
                Db.Complete();
                return Ok("Dodali ste novu liniju!");
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Linijas/5
        [AllowAnonymous]

        public IHttpActionResult DeleteLinija(int id)
        {
            Linija linija = db.Linije.Where(x => x.RedniBroj == id.ToString()).FirstOrDefault();
            if (linija == null)
            {
                return NotFound();
            }

            db.Linije.Remove(linija);
            db.SaveChanges();

            return Ok();
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