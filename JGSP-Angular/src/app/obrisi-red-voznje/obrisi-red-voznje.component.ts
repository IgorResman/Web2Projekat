import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { RedVoznjeService } from '../services/red-voznje.service';
@Component({
  selector: 'app-obrisi-red-voznje',
  templateUrl: './obrisi-red-voznje.component.html',
  styleUrls: ['./obrisi-red-voznje.component.css']
})
export class ObrisiRedVoznjeComponent implements OnInit {
  constructor(private redVoznjeService: RedVoznjeService, private fb: FormBuilder) { }
  RedGroup = this.fb.group({
    id: ['', Validators.required]
  });

  ngOnInit() { }

  Delete() {
    this.redVoznjeService.DeleteRedVoznje(this.RedGroup.get('id').value).subscribe();
  }
}
