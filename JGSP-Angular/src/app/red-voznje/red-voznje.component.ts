import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { error } from 'util';
import { LinijaService } from '../services/linija.service';
import { linija } from '../models/linija';
import { raspored } from '../models/raspored';
import { klasaPodaci } from '../models/klasa-podaci';

@Component({
  selector: 'app-red-voznje',
  templateUrl: './red-voznje.component.html',
  styleUrls: ['./red-voznje.component.css']
})
export class RedVoznjeComponent implements OnInit {
  polasci: string;
  ras: raspored = new raspored();
  linija: linija = new linija();
  klasa: klasaPodaci = new klasaPodaci();
  selectedLine: number;
  linijeZaView: number[];
  dani: string[] = ["Radni", "Subota", "Nedelja"];
  dan: string;
  text: string = "Klisa";

  constructor(private linijaService: LinijaService) { }

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
