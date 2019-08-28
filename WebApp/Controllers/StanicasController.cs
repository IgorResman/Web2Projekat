using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Stanicas")]
    public class StanicasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }

        public StanicasController(IUnitOfWork db)
        {
            Db = db;
        }

        // GET: api/Stanicas
        public IQueryable<Stanica> GetStanice()
        {
            return db.Stanice;
        }

        // GET: api/Stanicas/5
        [AllowAnonymous]
        [Route("GetStanicee")]
        public List<string> GetStanica()
        {
            List<Stanica> stanice = Db.Stanica.GetAll().AsQueryable().ToList();
            List<string> imena = new List<string>();

            stanice.ForEach(x =>
            {
                imena.Add(x.Naziv);
            });

            return imena;
        }
        [AllowAnonymous]
        [Route("Spoji/{linija}/{stanica}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetStanica(string linija, string stanica)
        {
            Linija lin = Db.Linija.GetAll().Where(x => x.RedniBroj == linija).FirstOrDefault();
            Stanica sta = Db.Stanica.GetAll().Where(x => x.Naziv == stanica).FirstOrDefault();

            lin.Stanice = new List<Stanica>();
            sta.Linije = new List<Linija>();

            lin.Stanice.Add(sta);
            sta.Linije.Add(lin);

            Db.Stanica.Update(sta);
            Db.Linija.Update(lin);
            Db.Complete();

            return Ok("");
        }

        // GET: api/Linijas/5   
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetStanica/{linijaBroj}")]
        public IHttpActionResult GetStanica(string linijaBroj)
        {
            List<Linija> sveLinije = Db.Linija.GetAll().ToList();
            Linija izabranaLinija = null;

            foreach (var l in sveLinije)
            {
                if (l.RedniBroj == linijaBroj)
                {
                    izabranaLinija = l;
                    break;
                }
            }

            if (izabranaLinija == null)
            {
                return NotFound();
            }

            List<Koordinate> listaKordinata = new List<Koordinate>();
            izabranaLinija.Stanice.ToList().ForEach(x =>
            {
                Koordinate k = new Koordinate() { x = x.X, y = x.Y, name = x.Naziv };
                listaKordinata.Add(k);
            });

            return Ok(listaKordinata);
        }

        // PUT: api/Stanicas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStanica(int id, Stanica stanica)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stanica.Id)
            {
                return BadRequest();
            }

            db.Entry(stanica).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StanicaExists(id))
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

        // POST: api/Stanicas
        [ResponseType(typeof(string))]
        [AllowAnonymous]
        public IHttpActionResult PostStanica(StanicaBinding stanica)
        {
            if (ModelState.IsValid)
            {
                Stanica st = new Stanica();
                st.Adresa = stanica.adresa;
                st.Naziv = stanica.naziv;
                st.X = stanica.x;
                st.Y = stanica.y;
                st.Linije = new List<Linija>();
                Db.Stanica.Add(st);
                Db.Complete();
                return Ok("Uspesno ste dodali novu stanicu!");
            }

            return BadRequest(ModelState);
        }
        [AllowAnonymous]
        // DELETE: api/Stanicas/5
        [Route("IzbrisiStanicu/{ime}/{nesto}/{nesto2}")]
        public IHttpActionResult GetStanica(string ime, string nesto, string nesto2)
        {
            Stanica stanica = db.Stanice.Where(x=> x.Naziv == ime).FirstOrDefault();
            if (stanica == null)
            {
                return NotFound();
            }

            db.Stanice.Remove(stanica);
            db.SaveChanges();

            return Ok();
        }

        //[HttpPost]
        //[Route("UploadImage/{username}")]
        //[AllowAnonymous]
        //public IHttpActionResult UploadImage(string username)
        //{
        //    var httpRequest = HttpContext.Current.Request;

        //    if (httpRequest.Files.Count > 0)
        //    {
        //        foreach (string file in httpRequest.Files)
        //        {
        //            //var user = UserManager.FindByName(username);
        //            var userStore = new UserStore<ApplicationUser>(db);
        //            var userManager = new UserManager<ApplicationUser>(userStore);
        //            ApplicationUser user = userManager.FindByName(username);
        //            //Sacuvati sliku u bazi i povezati je sa registrovanim userom


        //            //Passenger passenger = UnitOfWork.PassengerRepository.Get(id);

        //            //if (passenger == null)
        //            //{
        //            //    return BadRequest("User does not exists.");
        //            //}

        //            //if (passenger.ImageUrl != null)
        //            //{
        //            //    File.Delete(HttpContext.Current.Server.MapPath("~/UploadFile/" + passenger.ImageUrl));
        //            //}



        //            var postedFile = httpRequest.Files[file];
        //            string fileName = postedFile.FileName;
        //            var filePath = HttpContext.Current.Server.MapPath("~/SlikeKorisnika/" + fileName);

        //            Slika slika = null;
        //            IEnumerable<Slika> sveSlike = null;
        //            try
        //            {
        //                sveSlike = Db.Slika.GetAll();
        //            }
        //            catch (Exception e)
        //            {

        //            }

        //            bool korisnikImaSliku = false;

        //            if (sveSlike != null)
        //            {
        //                foreach (var s in sveSlike)
        //                {
        //                    if (s.Korisnik == user.Id)
        //                    {
        //                        korisnikImaSliku = true;
        //                        slika = s;
        //                        break;
        //                    }
        //                }

        //                if (korisnikImaSliku)
        //                {
        //                    Db.Slika.Update(slika);
        //                    Db.Complete();
        //                }
        //                else
        //                {
        //                    slika = new Slika() { ImageUrl = filePath, Korisnik = user.Id };
        //                    Db.Slika.Add(slika);
        //                    Db.Complete();
        //                }
        //            }
        //            else
        //            {
        //                slika = new Slika() { Id = 1, ImageUrl = filePath, Korisnik = user.Id };
        //                try
        //                {

        //                    Db.Slika.Add(slika);
        //                    Db.Complete();
        //                }
        //                catch (Exception e) { }
        //            }


        //            //UnitOfWork.PassengerRepository.Update(passenger);
        //            //UnitOfWork.Complete();


        //            postedFile.SaveAs(filePath);
        //        }

        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StanicaExists(int id)
        {
            return db.Stanice.Count(e => e.Id == id) > 0;
        }
    }

    class Koordinate
    {
        public double x { get; set; }
        public double y { get; set; }
        public string name { get; set; }
    }
}