import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { User } from '../models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() { }

  Login(user: User, form: NgForm) {
    let l = this.userService.LogIn(user.username, user.password);
    form.reset();
    this.router.navigate(["/home"]);
  }
}
