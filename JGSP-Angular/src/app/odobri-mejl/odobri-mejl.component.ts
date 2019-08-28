import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-odobri-mejl',
  templateUrl: './odobri-mejl.component.html',
  styleUrls: ['./odobri-mejl.component.css']
})
export class OdobriMejlComponent implements OnInit {
  mejloviZaView: number[];
  selectedLine: string;
  selectedMail: string;
  odgovor: string;
  slika: string;

  constructor(private userService: UserService, private fb: FormBuilder) { }

  ngOnInit() {
    this.userService.GetMails().subscribe((stanicesa) => {
      // this.selectedMail = stanicesa[0];
      this.mejloviZaView = stanicesa;
      err => console.log(err);
    });
  }

  Approve() {
    console.log(this.selectedMail);
    this.userService.Approve(this.selectedMail).subscribe((stanicesa) => {
      this.odgovor = stanicesa;
      err => console.log(err);
    });
  }

  GetImg() {
    //potrebno je poslati id korisnika i primiti sa servera sliku ako postoji
    this.userService.GetImage(this.selectedMail).subscribe((slika) => {
      this.slika = 'data:image/jpeg;base64,' + slika;
    }, err => {
      if (err.status === 400) {
        this.slika = null;
        alert("Korisnik nije dostavio sliku!");
      }
    });
  }
}
