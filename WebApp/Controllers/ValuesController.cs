using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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

            //coajovic.web@gmail.com //tastatura14
            StringBuilder sb = new StringBuilder();
            sb.Append("Korisniku " + appUser.UserName + " je odobren mejl!");

            try
            {
                MailHelper.Send(appUser.Email, "Podaci", sb.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

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
