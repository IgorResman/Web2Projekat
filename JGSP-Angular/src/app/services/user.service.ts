import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { RegUser } from '../osoba';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  base_url = "http://localhost:52295";
  user: string;

  constructor(private http: HttpClient) { }

  LogIn(username: string, password: string): Observable<boolean> | boolean {
    let isDone: boolean = false;
    let data = `username=${username}&password=${password}&grant_type=password`;
    let httpOptions = {
      headers: {
        "Content-type": "application/x-www-form-urlencoded"
      }
    }

    this.http.post<any>(this.base_url + "/oauth/token", data, httpOptions).subscribe(data => {
      localStorage.jwt = data.access_token;
      let jwtData = localStorage.jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData)
      let role = decodedJwtData.role
      this.user = decodedJwtData.unique_name;
    });

    isDone = localStorage.jwt != "undefined" ? true : false;

    return isDone;
  }

  Register(data: RegUser): Observable<any> {
    return this.http.post<any>(this.base_url + "/api/Account/Register", data);
  }

  RegisterImg(data: any, username: string): Observable<any> {
    return this.http.post<any>(this.base_url + "/api/Slikas/UploadImage/" + username, data);
  }

  GetMails(): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Values/GetZahtevi");
  }

  Approve(mejl: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Values/Odobri/" + mejl);
  }

  Verify(): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Values/Verifikovan");
  }

  GetImage(idKorisnika: string): Observable<any> {
    return this.http.get<any>(this.base_url + "/api/Slikas/GetSlika/" + idKorisnika);
  }
}
