import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { RedVoznje } from 'src/app/osoba';
import { LinijaService } from '../services/linija.service';
import { RedVoznjeService } from '../services/red-voznje.service';
@Component({
  selector: 'app-dodaj-red-voznje',
  templateUrl: './dodaj-red-voznje.component.html',
  styleUrls: ['./dodaj-red-voznje.component.css']
})
export class DodajRedVoznjeComponent implements OnInit {
  linijeZaView: number[];
  odgovor: string
  linija: string
  dani: string[] = ["Radni", "Subota", "Nedelja"];


  constructor(private linijaService: LinijaService, private redVoznjeService: RedVoznjeService, private fb: FormBuilder) { }
  redGroup = this.fb.group({

    dan: ['', Validators.required],
    linija: ['', Validators.required],
    red: ['', Validators.required],
  });

  ngOnInit() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
  }

  DodajRedVoznje() {
    let zaslanje: RedVoznje = this.redGroup.value;
    this.redGroup[2] = this.linija;
    this.redVoznjeService.AddRedVoznje(zaslanje).subscribe((odgovor) => {
      this.odgovor = odgovor;
      err => console.log(err);
    });
  }
}
