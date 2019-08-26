import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { RegUser } from '../models/reg-user';

@Injectable({
  providedIn: 'root'
})
export class KartaService {
  base_url = "http://localhost:52295";

  constructor(private http: HttpClient) { }


  Edit(data: RegUser): Observable<any> {
    return this.http.post<any>(this.base_url + "/api/Kartas/PromeniProfil", data);
  }

  GetUser(): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Kartas/DobaviUsera");
  }

  GetTicketPrice(tip: string, tipPutnika: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Kartas/GetKarta/" + tip + "/" + tipPutnika);
  }

  GetEditPrice(tip: string, tipPutnika: string, cena: number): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Kartas/GetKartaPromenaCene/" + tip + "/" + tipPutnika + "/" + cena);
  }

  GetBuyTicket(tipKarte: string, mejl: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Kartas/GetKartaKupi2/" + tipKarte + "/" + mejl);
  }

  GetBuyTicketUnregister(tipKarte: string, mejl: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Kartas/GetKartaKupi2/" + tipKarte + "/" + mejl);
  }

  GetCheckTicket(idKorisnika: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Kartas/GetProveri/" + idKorisnika);
  }
}
