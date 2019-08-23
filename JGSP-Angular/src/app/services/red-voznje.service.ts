import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { RedVoznje } from '../osoba';

@Injectable({
  providedIn: 'root'
})
export class RedVoznjeService {
  base_url = "http://localhost:52295";

  constructor(private http: HttpClient) { }

  AddRedVoznje(red: RedVoznje): Observable<any> {
    return this.http.post<any>(this.base_url + "/api/Redovi/dodajRed", red);
  }


  DeleteRedVoznje(id: number): Observable<any> {
    return this.http.delete<any>(this.base_url + "/api/RedVoznjes/" + id);
  }
}
