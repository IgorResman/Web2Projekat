import { Component, OnInit } from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Stanica } from 'src/app/osoba';
import { AuthHttpService } from 'src/app/services/auth.service';
import { StanicaService } from '../services/stanica.service';
import { LinijaService } from '../services/linija.service';
@Component({
  selector: 'app-spoji-stanica-linija',
  templateUrl: './spoji-stanica-linija.component.html',
  styleUrls: ['./spoji-stanica-linija.component.css']
})
export class SpojiStanicaLinijaComponent implements OnInit {
  linijeZaView: number[];
  staniceZaView: number[];
  selectedLine: string;
  slectedStanica: string;
  odgovor: string;
  constructor(private linijaService: LinijaService, private stanicaService: StanicaService, private fb: FormBuilder) { }

  ngOnInit() {

    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
    this.stanicaService.GetStations().subscribe((stanicesa) => {
      this.staniceZaView = stanicesa;
      err => console.log(err);
    });
  }

  Connect() {
    this.stanicaService.GetConnect(this.selectedLine, this.slectedStanica).subscribe((odg) => {
      this.odgovor = odg;
      err => console.log(err);
    });
  }
}
