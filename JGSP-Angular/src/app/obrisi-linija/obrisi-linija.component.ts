import { Component, OnInit } from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Stanica } from 'src/app/osoba';
import { AuthHttpService } from 'src/app/services/auth.service';
import { LinijaService } from '../services/linija.service';
@Component({
  selector: 'app-obrisi-linija',
  templateUrl: './obrisi-linija.component.html',
  styleUrls: ['./obrisi-linija.component.css']
})
export class ObrisiLinijaComponent implements OnInit {
  linijeZaView: number[];
  odgovor: string;
  selectedLine: number;
  constructor(private linijaService: LinijaService, private fb: FormBuilder) { }

  ngOnInit() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linijeZaView = linijesabekenda;
      err => console.log(err);
    });
  }

  Delete() {
    this.linijaService.DeleteLine(this.selectedLine).subscribe();
  }

}
