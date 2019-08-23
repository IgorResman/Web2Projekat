using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Hubs;

namespace WebApp.Controllers
{
    public class LokacijaController : ApiController
    {
        private LokacijaVozilaHub hub;

        public LokacijaController(LokacijaVozilaHub hub)
        {
            this.hub = hub;
        }
    }
}
