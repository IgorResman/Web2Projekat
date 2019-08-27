import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthHttpService } from 'src/app/services/auth.service';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { KartaService } from '../services/karta.service';
import { Profil } from '../models/profil';
import { RegUser } from '../models/reg-user';
@Component({
  selector: 'app-moj-profil',
  templateUrl: './moj-profil.component.html',
  styleUrls: ['./moj-profil.component.css']
})
export class MojProfilComponent implements OnInit {
  tipovi: string[] = ["Admin", "Student", "Penzioner", "Obican"];
  tip: string;
  profil: Profil;
  registacijaForm = this.fb.group({
    name: ['', Validators.required],
    surname: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
    email: ['', Validators.required],
    date: ['', Validators.required],
    tip: ['', Validators.required]
  });

  constructor(private kartaService: KartaService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.kartaService.GetUser().subscribe((profil) => {
      this.profil = profil;
      err => console.log(err);

      this.registacijaForm.patchValue({
        name: this.profil.Name,
        surname: this.profil.Surname,
        date: this.profil.Datum,
        password: this.profil.Password,
        confirmPassword: this.profil.ConfirmPassword,
        email: this.profil.Email,
        username: this.profil.UserName,
        tip: this.profil.Tip
      });
    });
  }

  onSubmit() {
    let regModel: RegUser = this.registacijaForm.value;
    this.kartaService.Edit(regModel).subscribe()
    this.router.navigate(["/home"])
    //form.reset();
  }
}
