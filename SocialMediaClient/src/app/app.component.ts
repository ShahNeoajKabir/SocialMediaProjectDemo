import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './Model/Token';
import { LoginService } from './Service/Login/login.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'client';
  User:any;
  constructor(private _loginService:LoginService){}
  ngOnInit() {
    this.setCurrentUser();


  }

  setCurrentUser(){
    const user:User=JSON.parse(localStorage.getItem('user') || ('user'));
    this._loginService.setCurrentuser(user);
  }




  }


