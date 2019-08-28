using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Hubs;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [RoutePrefix("api/Lokacija")]
    public class LokacijaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private LokacijaVozilaHub hub;
        public IUnitOfWork Db { get; set; }

        public LokacijaController(LokacijaVozilaHub hub, IUnitOfWork db)
        {
            this.hub = hub;
            this.Db = db;
        }


        [Route("StaniceZaHub")]
        public IHttpActionResult StaniceZaHub(LinijaZaHub lin)
        {
            List<Linija> listaLinija = Db.Linija.GetAll().ToList();
            Linija linija = null;

            foreach (var l in listaLinija)
            {
                if (l.RedniBroj == lin.imeLinije)
                {
                    linija = l;
                    break;
                }
            }

            List<Stanica> listaStanica = linija.Stanice.ToList();

            hub.DodajStanice(listaStanica);
            return Ok("Pronadjene su stanice");
        }
    }
}
