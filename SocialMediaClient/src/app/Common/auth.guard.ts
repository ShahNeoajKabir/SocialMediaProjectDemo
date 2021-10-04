import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LoginService } from '../Service/Login/login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private loginservice:LoginService , private toast:ToastrService){}
  canActivate(): Observable<boolean> {
      return this.loginservice.CurrentUser$.pipe(
        map((user)=>{
          if(user) return true;
          else{
            this.toast.error("You Shall not Pass!!");
            return false;
          }
          
        })
      )
  }
  
}
