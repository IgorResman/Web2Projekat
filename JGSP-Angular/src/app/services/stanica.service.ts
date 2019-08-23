import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { Stanica } from '../osoba';

@Injectable({
  providedIn: 'root'
})
export class StanicaService {
  base_url = "http://localhost:52295";

  constructor(private http: HttpClient) { }


  AddStation(stanica: Stanica): Observable<any> {
    return this.http.post<any>(this.base_url + "/api/Stanicas", stanica);
  }

  DeleteStation(ime: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Stanicas/IzbrisiStanicu/" + ime + "/a" + "/a");
  }

  GetStations(): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Stanicas/GetStanicee");
  }

  GetConnect(linija: string, stanica: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Stanicas/Spoji/" + linija + "/" + stanica);
  }

  GetStationCoord(idStanice: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Stanicas/GetStanica/" + idStanice);
  }

}
