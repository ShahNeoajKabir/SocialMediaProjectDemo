import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/Model/Member';
import { Registration } from 'src/app/Model/Registration';
import { LoginService } from 'src/app/Service/Login/login.service';
import { MemberService } from 'src/app/Service/Member/member.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  model:any={};
  member:Member;
  constructor(public _loginService:LoginService ,private _userService:MemberService, private _router:Router , 
    private _activate:ActivatedRoute,
    private _toaster:ToastrService) { }

  ngOnInit(): void {
  }


  
  Login(){
    this._loginService.Login(this.model).subscribe(res=>{
      this._router.navigateByUrl('/Member');
    },er=>{
      this._toaster.error(er.error);
    })
   }



   
   
   Logedout(){
     this._loginService.Logout();
     this._router.navigateByUrl('/');
     
   }

}
