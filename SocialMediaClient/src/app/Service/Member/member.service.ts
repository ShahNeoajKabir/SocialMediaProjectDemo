import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Member } from 'src/app/Model/Member';
import { PaginatedResult } from 'src/app/Model/pagination';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  url=environment.ApiUrl+'User/';
  member:Member[]=[];
  paginatedResult:PaginatedResult<Member[] | null> = new PaginatedResult<Member[]>();

  constructor(private http:HttpClient) { }

  public GetAll(page:number ,itemsPerPage:number){
    // if(this.member.length>0) return of(this.member);
    let params=new HttpParams();

    if(page!==null && itemsPerPage!==null){
      params=params.append('pageNumber', page.toString());
      params=params.append('pageSize', itemsPerPage.toString());

    }
    return this.http.get<Member[]>(this.url+'GetAll',{observe:'response',params}).pipe(
      map(response=>{
        this.paginatedResult.result=response.body;
        if(response.headers.get('Pagination')!==null){
          this.paginatedResult.pagination=JSON.parse(response.headers.get('Pagination')||('Pagination'));
        }
        return this.paginatedResult;
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


  setMainPhoto(photoId: number) {
    return this.http.put(this.url + 'set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.url + 'users/delete-photo/' + photoId);
  }
}
