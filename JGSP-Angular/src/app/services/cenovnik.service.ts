import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { CenovnikBindingModel } from '../models/cenovnik-binding-model';

@Injectable({
  providedIn: 'root'
})
export class CenovnikService {
  base_url = "http://localhost:52295"

  constructor(private http: HttpClient) { }

  AddCenovnik(cenovnik: CenovnikBindingModel): Observable<any> {
    return this.http.post<any>(this.base_url + "/PromeniCenovnik", cenovnik);
  }

  DeleteCenovnik(id: number): Observable<any> {
    return this.http.delete<any>(this.base_url + "/api/Cenovniks/" + id);
  }
}
