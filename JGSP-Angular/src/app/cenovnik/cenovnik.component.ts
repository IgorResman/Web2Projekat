import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { KartaService } from '../services/karta.service';
@Component({
  selector: 'app-cenovnik',
  templateUrl: './cenovnik.component.html',
  styleUrls: ['./cenovnik.component.css']
})
export class CenovnikComponent implements OnInit {
  tipovi: string[] = ["Dnevna", "Mesecna", "Godisnja", "Vremenska"];
  tipoviPutnika: string[] = ["Student", "Penzioner", "Obican"];
  tip: string;
  tipPutnika: string;
  cena: number;
  constructor(private kartaService: KartaService) { }

  ngOnInit() { }

  TicketPrice() {
    this.kartaService.GetTicketPrice(this.tip, this.tipPutnika).subscribe((cena) => {
      this.cena = cena;
      err => console.log(err);
    });
  }
}
