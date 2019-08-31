using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Kartas")]
    public class KartasController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }

        public KartasController(IUnitOfWork db)
        {
            Db = db;
        }


        // GET: api/Kartas
        public IQueryable<Karta> GetKarte()
        {
            return db.Karte;
        }

        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetProveri/{IdKorisnika}")]
        public IHttpActionResult GetProveri(string IdKorisnika)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser u = userManager.FindByName(IdKorisnika);

            Karta karta = new Karta();
            List<Karta> karte = Db.Karta.GetAll().ToList();
            //var user = UserManager.FindByName(IdKorisnika);

            string retVal = String.Empty;

            karte.ForEach(x =>
            {
                if (x.ApplicationUserId != null && x.ApplicationUserId.Equals(u.Id))
                {
                    karta = x;
                }
            });
            
            if (karta != null)
            {
                switch (karta.Tip)
                {
                    case "Dnevna":
                        var datumKarte = karta.VaziDo;
                        var pocetakSledecegDana = new DateTime(datumKarte.Year, datumKarte.Month, datumKarte.AddDays(1).Day);

                        if (pocetakSledecegDana > DateTime.UtcNow)
                        {
                            retVal = "Vazi vam karta";
                        }
                        else
                        {
                            retVal = "Ne vazi vam karta!";
                        }
                        break;

                    case "Mesecna":
                        datumKarte = karta.VaziDo;
                        var startOfMonth = new DateTime(datumKarte.Year, datumKarte.Month, 1);
                        var DaysInMonth = DateTime.DaysInMonth(datumKarte.Year, datumKarte.Month);
                        var lastDay = new DateTime(datumKarte.Year, datumKarte.Month, DaysInMonth);

                        if (lastDay > DateTime.UtcNow)
                        {
                            retVal = "Vazi vam karta";
                        }
                        else
                        {
                            retVal = "Ne vazi vam karta!";
                        }
                        break;

                    case "Godisnja":
                        var now = karta.VaziDo;
                        var startOfYear = new DateTime(now.Year, 1, 1);
                        var nextYear = new DateTime(startOfYear.AddYears(1).Year, 1, 1);

                        if (nextYear > DateTime.UtcNow)
                        {
                            retVal = "Vazi vam karta";
                        }
                        else
                        {
                            retVal = "Ne vazi vam karta!";
                        }
                        break;

                    case "Vremenska":
                        var vremeKarte = karta.VaziDo.Hour;
                        var nextHour = vremeKarte + 1;

                        if (nextHour > DateTime.UtcNow.Hour)
                        {
                            retVal = "Vazi vam karta";
                        }
                        else
                        {
                            retVal = "Ne vazi vam karta!";
                        }
                        break;
                    default:
                        retVal = "Ne vazi vam karta!";
                        break;
                }

                return Ok(retVal);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Kartas/5
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetKarta/{tipKarte}/{tipKupca}")]
        public IHttpActionResult GetKartaCena(string tipKarte, string tipKupca)
        {
            List<CenaKarte> karte = Db.CenaKarte.GetAll().ToList();

            if (karte == null)
            {
                return NotFound();
            }

            List<Cenovnik> cenovnici = Db.Cenovnik.GetAll().ToList();

            Cenovnik cen = Db.Cenovnik.GetAll().Where(t => t.VaziDo > DateTime.UtcNow && t.VaziOd < DateTime.UtcNow).FirstOrDefault();

            StringBuilder retVal = new StringBuilder();
            retVal.Append("Cena zeljene karte je : ");

            karte.ForEach(x =>
            {
                if (x.TipKarte == tipKarte && tipKupca == x.TipKupca && cen.IdCenovnik == x.CenovnikId)
                {
                    retVal.Append(x.Cena.ToString());
                }
            });

            retVal.Append(" RSD.");

            return Ok(retVal.ToString());
        }

        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetKartaPromenaCene/{tipKarte}/{tipKupca}/{cena}")]
        public IHttpActionResult GetKartaCena(string tipKarte, string tipKupca, int cena)
        {
            //POTREBNO JE PRAVITI NOVI CENOVNIK KADA SE PROMENI CENA KARTE
            List<CenaKarte> karte = Db.CenaKarte.GetAll().ToList();

            if (karte == null)
            {
                return NotFound();
            }

            List<Cenovnik> cenovnici = Db.Cenovnik.GetAll().ToList();
            Cenovnik cen = Db.Cenovnik.GetAll().Where(t => t.VaziDo > DateTime.UtcNow && t.VaziOd < DateTime.UtcNow).FirstOrDefault();

            StringBuilder retVal = new StringBuilder();
            retVal.Append("Cena zeljene karte je bila: ");
            karte.ForEach(x =>
            {
                if (x.TipKarte == tipKarte && tipKupca == x.TipKupca && cen.IdCenovnik == x.CenovnikId)
                {
                    retVal.Append(x.Cena.ToString());
                    x.Cena = cena;
                    Db.CenaKarte.Update(x);

                    Db.Complete();
                }
            });
            
            retVal.Append(" RSD.");

            retVal.Append("Sada je promenjena na : " + cena.ToString() + " rsd.");

            return Ok(retVal.ToString());
        }
        [AllowAnonymous]
        [ResponseType(typeof(Profil))]
        [Route("DobaviUsera")]
        public IHttpActionResult GetUsera()
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var id = User.Identity.GetUserId();
            ApplicationUser u = userManager.FindById(id);

            if (u == null)
            {
                return NotFound();
            }

            Profil p = new Profil()
            {
                Name = u.Name,
                Password = u.Password,
                Surname = u.Surname,
                Tip = u.Tip,
                Datum = u.Datum,
                ConfirmPassword = u.ConfirmPassword,
                Email = u.Email,
                UserName = u.UserName
            };

            return Ok(p);
        }
        [AllowAnonymous]
        [Route("PromeniProfil")]
        public IHttpActionResult PostKorisnika(RegisterBindingModel model)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var id = User.Identity.GetUserId();
            ApplicationUser u = userManager.FindById(id);

            if (u == null)
            {
                return NotFound();
            }

            u.Email = model.Email;
            u.Datum = model.Date;
            u.ConfirmPassword = model.ConfirmPassword;
            u.Password = model.Password;
            u.Name = model.Name;
            u.UserName = model.Email;
            u.Surname = model.Surname;
            u.Tip = model.Tip;

            db.Entry(u).State = EntityState.Modified;

            db.SaveChanges();
            return Ok();
        }
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("GetKartaKupi2/{tipKarte}/{mejl}")]
        public IHttpActionResult GetKarta(string tipKarte, string mejl)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            Karta novaKarta = new Karta();
            string tipKorisnika;
            var id = User.Identity.GetUserId();
            ApplicationUser u = userManager.FindById(id);

            if (u == null)
            {
                tipKorisnika = "Obican";
            }
            else
            {
                tipKorisnika = u.Tip;
            }

            float cena;
            StringBuilder retVal = new StringBuilder();

            List<Cenovnik> cenovnici = Db.Cenovnik.GetAll().ToList();
            Cenovnik cen = Db.Cenovnik.GetAll().Where(t => t.VaziDo > DateTime.UtcNow && t.VaziOd < DateTime.UtcNow && t.Aktuelan == true).FirstOrDefault();
            CenaKarte ck = Db.CenaKarte.GetAll().Where(t => t.TipKarte == tipKarte && t.TipKupca == tipKorisnika && t.CenovnikId == cen.IdCenovnik).FirstOrDefault();

            novaKarta.CenaKarteId = ck.IdCenaKarte;
            novaKarta.Tip = tipKarte;
            novaKarta.VaziDo = DateTime.UtcNow;

            if (u != null && u.Odobren == true)
            {
                novaKarta.ApplicationUserId = id;
                cena = ck.Cena;
                retVal.Append("Uspesno ste kupili " + tipKarte + "-u" + " kartu, po ceni od |" + cena.ToString() + "| rsd, hvala vam, vas gsp!");
                novaKarta.Cekirana = true;

                db.Dispose();
                Db.Karta.Add(novaKarta);
                Db.Complete();
            }
            else if (u != null && u.Odobren == false)
            {
                retVal.Append("Kontrolor vas nije prihvatio");
            }
            else if (u == null)
            {

                string mailSubject = "JGSP";
                string mailBody = $"Uspesno ste kupili kartu: {novaKarta.IdKarte} {Environment.NewLine} u {DateTime.UtcNow} {Environment.NewLine} Hvala na poverenju, JGSP!";
                try
                {
                    try
                    {
                        MailHelper.Send(mejl + "@gmail.com", mailSubject, mailBody);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }

                    cena = ck.Cena;
                    retVal.Append("Uspesno ste kupili " + tipKarte + "-u" + " kartu, po ceni od |" + cena.ToString() + "| rsd, hvala vam, vas gsp!");
                    novaKarta.Cekirana = true;

                    db.Dispose();
                    Db.Karta.Add(novaKarta);
                    Db.Complete();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return InternalServerError(e);
                }
            }

            if (ck == null)
            {
                return NotFound();
            }

            return Ok(retVal.ToString());
        }

        // PUT: api/Kartas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKarta(int id, Karta karta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != karta.IdKarte)
            {
                return BadRequest();
            }

            db.Entry(karta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KartaExists(id))
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

        // POST: api/Kartas
        [ResponseType(typeof(Karta))]
        public IHttpActionResult PostKarta(Karta karta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Karte.Add(karta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = karta.IdKarte }, karta);
        }

        // DELETE: api/Kartas/5
        [ResponseType(typeof(Karta))]
        public IHttpActionResult DeleteKarta(int id)
        {
            Karta karta = db.Karte.Find(id);
            if (karta == null)
            {
                return NotFound();
            }

            db.Karte.Remove(karta);
            db.SaveChanges();

            return Ok(karta);
        }

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult TransakcijaKarta(string idTransakcije)
        {
            //proveriti korisnika i za njegovu poslednju kartu dodati id transakcije u tabelu
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var id = User.Identity.GetUserId();
            ApplicationUser u = userManager.FindById(id);

            Karta karta = new Karta()
            {
                Cekirana = false,
                Tip = "Dnevna",
                VaziDo = DateTime.UtcNow.AddDays(1),
                ApplicationUserId = id,
                CenaKarteId = 4,
                idTransakcije = idTransakcije
            };

            db.Karte.Add(karta);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException db)
            {
                return NotFound();
            }

            return Ok("Sacuvano");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KartaExists(int id)
        {
            return db.Karte.Count(e => e.IdKarte == id) > 0;
        }
    }
}