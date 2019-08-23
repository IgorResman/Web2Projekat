import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { LinijaService } from '../services/linija.service';
@Component({
  selector: 'app-dodaj-liniju',
  templateUrl: './dodaj-liniju.component.html',
  styleUrls: ['./dodaj-liniju.component.css']
})
export class DodajLinijuComponent implements OnInit {

  constructor(private linijaService: LinijaService, private fb: FormBuilder) { }
  LinijaGroup = this.fb.group({
    broj: ['', Validators.required]
  });

  ngOnInit() { }

  Add() {
    this.linijaService.AddLine(this.LinijaGroup.get('broj').value).subscribe();
  }
}
