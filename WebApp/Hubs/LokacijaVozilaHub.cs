using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Hubs
{
    [HubName("notifications")]
    public class LokacijaVozilaHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<LokacijaVozilaHub>();

        private static Timer timer = new Timer();
        private IUnitOfWork unitOfWork;

        public LokacijaVozilaHub(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void GetTime()
        {
            //Svim klijentima se salje setRealTime poruka
            Clients.All.setRealTime(DateTime.Now.ToString("h:mm:ss tt"));
        }

        public void StartLocationServerUpdates()
        {
            timer.Interval = 1000;
            timer.Start();
            timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
             Lokacija();
        }

        private void Lokacija()
        {
            StringBuilder busData = new StringBuilder("");
            var stanice = unitOfWork.Stanica.GetAll();
            var linijaBr3 = unitOfWork.Linija.Get(6);

            var s = stanice.ToList();

            var ss = s[2];
            busData.Append($"{ss.X}_{ss.Y};");
            
            //foreach(var s in stanice)
            //{
            //    var listaLinijaNaStaniciS = s.Linije.ToList();
            //    foreach(var lin in listaLinijaNaStaniciS)
            //    {
            //        if(lin.Id == linijaBr3.Id)
            //        {
            //            busData.Append($"{s.X}_{s.Y};");
            //            break;
            //        }
            //    }
            //}


            Clients.Group("Admins").getBusData(busData.ToString());
        }

        public void StopLocationServerUpdates()
        {
            timer.Stop();
        }

        public void NotifyAdmins(int clickCount)
        {
            hubContext.Clients.Group("Admins").userClicked($"Clicks: {clickCount}");
        }

        public override Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, "Admins");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, "Admins");

            return base.OnDisconnected(stopCalled);
        }
    }
}