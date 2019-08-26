import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { error } from 'util';
import { LinijaService } from '../services/linija.service';
import { linija } from '../models/linija';
@Component({
  selector: 'app-linije',
  templateUrl: './linije.component.html',
  styleUrls: ['./linije.component.css']
})
export class LinijeComponent implements OnInit {

  constructor(private linijaService: LinijaService) { }
  linija: linija = new linija();
  ngOnInit() { }

  OnGetLinije() {
    this.linijaService.GetLines().subscribe((linijesabekenda) => {
      this.linija.linije = linijesabekenda;
      err => console.log(err);
    });
  }
}
