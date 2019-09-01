import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { StanicaService } from '../services/stanica.service';
import { LinijaService } from '../services/linija.service';
import { Stanica } from '../models/stanica';
@Component({
  selector: 'app-dodaj-stanicu',
  templateUrl: './dodaj-stanicu.component.html',
  styleUrls: ['./dodaj-stanicu.component.css']
})
export class DodajStanicuComponent implements OnInit {
  linijeZaView: number[];
  odgovor: string


  constructor(private linijaService: LinijaService, private stanicaService: StanicaService, private fb: FormBuilder) { }
  StanicaGroup = this.fb.group({
    adresa: ['', Validators.required],
    naziv: ['', Validators.required],
    x: ['', Validators.required],
    y: ['', Validators.required],

  });

  ngOnInit() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
  }

  DodajStanicu() {
    let zaslanje: Stanica = this.StanicaGroup.value;

    this.stanicaService.AddStation(zaslanje).subscribe((odgovor) => {
      this.odgovor = odgovor;
      err => console.log(err);
    });
  }
}
