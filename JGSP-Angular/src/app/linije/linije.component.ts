import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { error } from 'util';
import { raspored, linja } from 'src/app/osoba';
import { LinijaService } from '../services/linija.service';
@Component({
  selector: 'app-linije',
  templateUrl: './linije.component.html',
  styleUrls: ['./linije.component.css']
})
export class LinijeComponent implements OnInit {

  constructor(private linijaService: LinijaService) { }
  linija: linja = new linja();
  ngOnInit() { }

  OnGetLinije() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linija.linije = linijesabekenda;
      err => console.log(err);
    });
  }
}
