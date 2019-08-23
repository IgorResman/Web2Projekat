import { Component, OnInit, Input, NgZone } from '@angular/core';
import { GeoLocation } from '../map/map-model/geolocation';
import { MarkerInfo } from '../map/map-model/marker-info.model';
import { AuthHttpService } from '../services/auth.service';
import { Polyline } from '../map/map-model/polyline';
import { raspored, klasaPodaci, linja } from 'src/app/osoba';
import { LokacijaVozilaService } from '../services/lokacija.vozila.service';
import { Router } from '@angular/router';
import { LinijaService } from '../services/linija.service';

@Component({
  selector: 'app-lokacija-vozila',
  templateUrl: './lokacija-vozila.component.html',
  styleUrls: ['./lokacija-vozila.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}']
})
export class LokacijaVozilaComponent implements OnInit {
  isConnected: Boolean;
  locations: string[];
  polasci: string;
  ras: raspored = new raspored();
  linija: linja = new linja();
  klasa: klasaPodaci = new klasaPodaci();
  selectedLine: number;
  linijeZaView: number[];
  dani: string[] = ["Radni", "Subota", "Nedelja"];
  dan: string;
  text: string = "Klisa";
  markeri: MarkerInfo[] = [];
  busKordinate: string[];
  autobusMarker: MarkerInfo;
  public polylineMoje: Polyline;

  constructor(private lokacijaServis: LokacijaVozilaService, private linijaService: LinijaService, private ngZone: NgZone, private router: Router) {
    this.isConnected = false;
    this.locations = [];

  }

  ngOnInit() {
    this.polylineMoje = new Polyline([], 'blue', { url: "assets/lasta.jpg", scaledSize: { width: 50, height: 50 } });
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
    this.checkConnection();
    this.subscribeForLocations();
    this.lokacijaServis.registerForLocation();
  }

  private checkConnection() {
    this.lokacijaServis.startConnection().subscribe(e => {
      this.isConnected = e;
      console.log(e);
      if (e) {
        this.lokacijaServis.StartTimer();
      }
    });
  }

  private subscribeForLocations() {
    this.lokacijaServis.notificationReceived.subscribe(l => this.onNotification(l));
  }

  public onNotification(notification: string) {

    this.ngZone.run(() => {
      console.log(notification);
      let busevi = notification.split(";");
      //busevi.forEach(element => {
      let temp = busevi[0].split("_");
      //if(temp[0] == this.selectedLinija)
      this.busKordinate = temp;
      if (this.busKordinate != undefined) {
        var x = parseFloat(this.busKordinate[1].replace(',', '.'));
        var y = parseFloat(this.busKordinate[0].replace(',', '.'));
        this.autobusMarker = new MarkerInfo(new GeoLocation(x, y), "assets/lasta.jpg", "", "", "");
        this.polylineMoje.addLocation(new GeoLocation(+this.busKordinate[1], +this.busKordinate[0]));
        this.markeri.push(this.autobusMarker);
      }
    }
    );
  }


  OnGetPolasci() {
    this.lokacijaServis.StartTimer();
    this.lokacijaServis.notificationReceived.subscribe(l => this.onNotification(l));
    //this.polylineMoje.addLocation(new GeoLocation(+this.busKordinate[1], +this.busKordinate[0]));
  }
}

