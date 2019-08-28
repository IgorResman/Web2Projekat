import { Injectable } from '@angular/core';
import { LinijaZaHub } from '../models/linija-za-hub';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HubService {
  base_url = "http://localhost:52295";

  constructor(private http: HttpClient) { }

  StanicaZaHub(lin: LinijaZaHub): Observable<any> {
    let httpOptions = {
      headers: {
        "Content-type": "application/json"
      }
    }
    return this.http.post<any>(this.base_url + "/api/Lokacija/StaniceZaHub", lin, httpOptions);
  }
}
