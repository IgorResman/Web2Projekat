import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LinijaService {
  base_url = "http://localhost:52295";

  constructor(private http: HttpClient) { }

  AddLine(broj: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Linijas/GetLinijaDodaj/" + broj);
  }

  DeleteLine(id: number): Observable<any> {
    return this.http.delete<any>(this.base_url + "/api/Linijas/" + id);
  }

  GetDepartures(id: number, dan: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Linijas/GetLinija/" + id + "/" + dan);
  }

  GetLines(): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Linijas/");
  }

  //samo da se iscita json na serveru i popuni baza
  ParseJSON(id: number, dan: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Linijas/GetLinija/" + id + "/" + dan + "/" + "str");
  }
}
