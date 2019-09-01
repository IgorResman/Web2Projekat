import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { RegUser } from '../models/reg-user';
@Component({
  selector: 'app-registracija',
  templateUrl: './registracija.component.html',
  styleUrls: ['./registracija.component.css']
})
export class RegistracijaComponent implements OnInit {

  registacijaForm = this.fb.group({
    name: ['', Validators.required],
    surname: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
    email: ['', Validators.email],
    date: ['', Validators.required],
    tip: ['', Validators.required]
  });

  constructor(private userService: UserService, private fb: FormBuilder, private router: Router) { }
  tipovi: string[] = ["Obican", "Student", "Penzioner"];
  tip: string;
  slika: File = null;

  ngOnInit() { }

  onSubmit() {
    let regModel: RegUser = this.registacijaForm.value;
    this.userService.Register(regModel).subscribe(x => {
      if (this.slika != null) {
        const fData: FormData = new FormData();
        fData.append('Img', this.slika, this.slika.name);
        console.log(fData)
        this.userService.RegisterImg(fData, regModel.username).subscribe();
      }
      else {
        alert("Uspesno ste se registrovali");
        this.router.navigate(["/login"])
      }
    });
  }

  DaLiJeStudentILIPenzioner() {
    // console.log(this.registacijaForm.value.tip);
    return this.registacijaForm.value.tip == 'Student' || this.registacijaForm.value.tip == 'Penzioner' ? true : false;
  }

  onFileSelected(event) {
    this.slika = <File>event.target.files[0];
    console.log(this.slika);
  }

  sendWithImg() { }

}
