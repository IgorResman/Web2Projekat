import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  verifikovan: string;
  role: any;
  bul: boolean
  constructor(private userService: UserService, private ruter: Router) { }

  ngOnInit() {
    if (localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != "") {
      let jwtData = localStorage.jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData)
      this.role = decodedJwtData.nameid
    }
    this.verifikovan = null;
  }

  IsJWTUndefined(): boolean {
    return localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != "";
  }

  LogOut() {
    localStorage.setItem('jwt', undefined);
    this.ruter.navigate(['home']);
  }

  IsControllor() {
    return this.role == 'Kontrolor' ? true : false;
  }

  IsAdmin() {
    return this.role == 'Admin' ? true : false;
  }

  IsVerified() {
    this.userService.Verify().subscribe((response) => {
      console.log(response);
      this.verifikovan = response;
    });
  }
}
