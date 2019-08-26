import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { CenovnikService } from '../services/cenovnik.service';
import { CenovnikBindingModel } from '../models/cenovnik-binding-model';
@Component({
  selector: 'app-cenovnik-dodaj',
  templateUrl: './cenovnik-dodaj.component.html',
  styleUrls: ['./cenovnik-dodaj.component.css']
})
export class CenovnikDodajComponent implements OnInit {

  constructor(private cenovnikService: CenovnikService, private fb: FormBuilder) { }

  odgovor: string;
  cenovnikGroup = this.fb.group({
    mesecna: ['', Validators.required],
    godisnja: ['', Validators.required],
    vremenska: ['', Validators.required],
    dnevna: ['', Validators.required],
    vaziDo: ['', Validators.required],
    vaziOd: ['', Validators.required],
    popustPenzija: ['', Validators.required],
    popustStudent: ['', Validators.required],
    id: ['', Validators.required]
  });

  ngOnInit() { }

  CreateCenovnik() {
    let cenovnik: CenovnikBindingModel = this.cenovnikGroup.value;
    this.cenovnikService.AddCenovnik(cenovnik).subscribe();
  }
}
