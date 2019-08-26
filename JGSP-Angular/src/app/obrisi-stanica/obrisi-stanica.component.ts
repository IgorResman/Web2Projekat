import { Component, OnInit } from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { AuthHttpService } from 'src/app/services/auth.service';
import { StanicaService } from '../services/stanica.service';

@Component({
  selector: 'app-obrisi-stanica',
  templateUrl: './obrisi-stanica.component.html',
  styleUrls: ['./obrisi-stanica.component.css']
})
export class ObrisiStanicaComponent implements OnInit {
  staniceZaView: number[];
  selectedLine: string;
  slectedStanica: string;
  odgovor: string;
  constructor(private stanicaService: StanicaService, private fb: FormBuilder) { }

  ngOnInit() {
    this.stanicaService.GetStations().subscribe((stanicesa) => {
      this.staniceZaView = stanicesa;
      err => console.log(err);
    });
  }

  Delete() {
    this.stanicaService.DeleteStation(this.slectedStanica).subscribe();
  }
}
