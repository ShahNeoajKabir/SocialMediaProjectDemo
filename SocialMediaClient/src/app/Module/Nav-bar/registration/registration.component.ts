import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Registration } from 'src/app/Model/Registration';
import { LoginService } from 'src/app/Service/Login/login.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  register:any={};

  constructor(public _loginService:LoginService , private _router:Router , private _toaster:ToastrService) { }

  ngOnInit(): void {
  }

  Registration(){
    this._loginService.Registration(this.register).subscribe(res=>{
      this._toaster.success("Registration Successfull");

    },er=>{
      this._toaster.error(er.error);

    })

  }

}
