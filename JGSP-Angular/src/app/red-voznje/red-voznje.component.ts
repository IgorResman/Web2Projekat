import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { error } from 'util';
import { raspored, klasaPodaci } from 'src/app/osoba';
import { linja } from 'src/app/osoba';
import { LinijaService } from '../services/linija.service';

@Component({
  selector: 'app-red-voznje',
  templateUrl: './red-voznje.component.html',
  styleUrls: ['./red-voznje.component.css']
})
export class RedVoznjeComponent implements OnInit {

  polasci: string;
  constructor(private linijaService: LinijaService) { }
  ras: raspored = new raspored();
  linija: linja = new linja();
  klasa: klasaPodaci = new klasaPodaci();
  selectedLine: number;
  linijeZaView: number[];
  dani: string[] = ["Radni", "Subota", "Nedelja"];
  dan: string;
  text: string = "Klisa";

  ngOnInit() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
  }


  OnGetLines() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
  }

  OnGetDepartures() {
    this.linijaService.GetDepartures(this.selectedLine, this.dan).subscribe((raspored1) => {
      this.ras.polasci = raspored1;
      err => console.log(err);
    });
  }

  ParseJSON() {
    this.linijaService.ParseJSON(this.selectedLine, this.dan).subscribe();
  }
}