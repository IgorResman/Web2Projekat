using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IUnitOfWork Db { get; set; }
        public ValuesController(IUnitOfWork db)
        {
            Db = db;

        }
        public ValuesController()
        {
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [AllowAnonymous]

        [Route("GetZahtevi")]
        public List<string> GetValues()
        {
            List<ApplicationUser> accounts;
            accounts = db.Users.AsQueryable().ToList();
            List<string> usernames = new List<string>();

            accounts.ForEach(x =>
            {
                if (!x.Odobren)
                    usernames.Add(x.UserName);
            });
            
            return usernames;
        }
        // POST api/values
        [AllowAnonymous]

        [Route("Odobri/{mejl}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetValues(string mejl)
        {
            List<ApplicationUser> accounts;
            accounts = db.Users.AsQueryable().ToList();
            ApplicationUser appUser = new ApplicationUser();

            accounts.ForEach(x =>
            {
                if (x.UserName.Equals(mejl))
                    appUser = x;
            });

            appUser.Odobren = true;
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            string email = mejl.Replace('-', '.');
            MailMessage mail = new MailMessage("ajovke@gmail.com", appUser.Email);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential("ajovke@gmail.com", "qcfu xays czwu bopw");    //iymr rzbn gpfs bpbg
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";

            mail.Subject = "JGSP";
            mail.Body = $"Vasa registracija je odobrena - {DateTime.Now}. {Environment.NewLine} Sada mozete kupovati karte na nasem servisu. {Environment.NewLine}";
            client.Send(mail);

            return Ok("Odobrili ste mu registraciju!");
        }

        [AllowAnonymous]
        [Route("Verifikovan")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetVerifikovan()
        {
            List<ApplicationUser> accounts;
            accounts = db.Users.AsQueryable().ToList();
            ApplicationUser appUser = new ApplicationUser();
            var id = User.Identity.GetUserId();

            accounts.ForEach(x =>
            {
                if (x.Id.Equals(id))
                    appUser = x;
            });

            return appUser.Odobren ? Ok("Verifikovan") : Ok("Nije Verifikovan");
        }
    }
}
