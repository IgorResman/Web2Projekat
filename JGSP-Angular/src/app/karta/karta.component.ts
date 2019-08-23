import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { KartaService } from '../services/karta.service';
//import { user } from 'src/app/services/auth.service';

@Component({
  selector: 'app-karta',
  templateUrl: './karta.component.html',
  styleUrls: ['./karta.component.css']
})
export class KartaComponent implements OnInit {

  constructor(private kartaService: KartaService) { }
  tipovi: string[] = ["Dnevna", "Mesecna", "Godisnja", "Vremenska"];
  cenaKarte: number = 15;
  tip: string;
  tipPutnika: string;
  cena: number;
  vaziDo: string;
  user: string;
  mejl: string;
  ngOnInit() { }


  BuyTicket() {
    this.kartaService.GetBuyTicket(this.tip, "prazno").subscribe((vaziDo) => {
      this.vaziDo = vaziDo;
      alert(vaziDo);
      err => console.log(err);
    });

  }
}
