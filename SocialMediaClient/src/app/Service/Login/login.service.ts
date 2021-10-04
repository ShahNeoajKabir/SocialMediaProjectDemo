import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Registration } from 'src/app/Model/Registration';

import { User } from 'src/app/Model/Token';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  url=environment.ApiUrl+'Security/'
  private CurrentPropertySource=new ReplaySubject<User>(1);
  CurrentUser$=this.CurrentPropertySource.asObservable();

  constructor(private http:HttpClient) { }


  Registration(register:any){
    return this.http.post(this.url+'Registration',register);

  }

  Login(model:any){
    return this.http.post<User>(this.url+'Login',model).pipe(
      map((res:User)=>{
        const user=res;
        if(user){
         this.setCurrentuser(user);
        }
        
      })
     )
  }


  setCurrentuser(user:User){
    localStorage.setItem('user', JSON.stringify(user));
    this.CurrentPropertySource.next(user)
  }

  Logout(){
    localStorage.removeItem('user');
    this.CurrentPropertySource.next(undefined);
  }
}
