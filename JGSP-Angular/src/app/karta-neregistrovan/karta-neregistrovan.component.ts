import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { KartaService } from '../services/karta.service';

@Component({
  selector: 'app-karta-neregistrovan',
  templateUrl: './karta-neregistrovan.component.html',
  styleUrls: ['./karta-neregistrovan.component.css']
})
export class KartaNeregistrovanComponent implements OnInit {

  constructor(private kartaService: KartaService, private fb: FormBuilder) { }
  tipovi: string[] = ["Dnevna", "Mesecna", "Godisnja", "Vremenska"];

  tip: string;
  tipPutnika: string;
  cena: number;
  vaziDo: string;
  user: string;

  regGroup = this.fb.group({
    mejl: ['', Validators.required],
  });

  ngOnInit() { }

  BuyTicketUnregister() {
    this.kartaService.GetBuyTicket(this.tip, this.regGroup.get('mejl').value).subscribe((vaziDo) => {
      this.vaziDo = vaziDo;
    });
  }
}
