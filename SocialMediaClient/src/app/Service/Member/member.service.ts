import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Member } from 'src/app/Model/Member';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  url=environment.ApiUrl+'User/';
  member:Member[]=[];

  constructor(private http:HttpClient) { }

  public GetAll(){
    if(this.member.length>0) return of(this.member);
    return this.http.get<Member[]>(this.url+'GetAll').pipe(
      map(member=>{
        this.member=member; 
        return member;
      }
        )
    )
  }

  public GetByUserName(username:string|null){
    const member=this.member.find(p=>p.userName===username);
    if(member!==undefined) return of(member);

    return this.http.get<Member>(this.url+username);
  }

  updateMember(member: Member) {
    return this.http.put(this.url+"UpdateUser", member).pipe(
      map(() => {
        const index = this.member.indexOf(member);
        this.member[index] = member;
      })
    )
  }
}
