import { Component } from '@angular/core';
import { Osoba } from './models/osoba';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'NewProject';
  peraOsoba: Osoba = { name: "Pera", surname: "Varga" }
}
