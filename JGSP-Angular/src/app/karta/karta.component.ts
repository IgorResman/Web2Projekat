import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { KartaService } from '../services/karta.service';
import { Validators, FormBuilder } from '@angular/forms';
//import { user } from 'src/app/services/auth.service';

@Component({
  selector: 'app-karta',
  templateUrl: './karta.component.html',
  styleUrls: ['./karta.component.css']
})
export class KartaComponent implements OnInit {

  constructor(private kartaService: KartaService, private fb: FormBuilder) { }
  tipovi: string[] = ["Dnevna", "Mesecna", "Godisnja", "Vremenska"];
  cenaKarte: number = 15;
  tip: string;
  tipPutnika: string;
  cena: number;
  vaziDo: string;
  user: string;
  mejl: string;


  regGroup = this.fb.group({
    mejl: ['', Validators.required],
  });

  ngOnInit() { }

  IsJWTUndefined(): boolean {
    return localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != "";
  }

  BuyTicket() {
    this.kartaService.GetBuyTicket(this.tip, "prazno").subscribe((vaziDo) => {
      this.vaziDo = vaziDo;
      alert(vaziDo);
      err => console.log(err);
    });
  }

  BuyTicketUnregister() {
    this.kartaService.GetBuyTicket(this.tip, this.regGroup.get('mejl').value).subscribe((vaziDo) => {
      this.vaziDo = vaziDo;
    });
  }
}
