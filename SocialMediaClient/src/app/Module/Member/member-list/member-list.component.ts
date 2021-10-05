import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Member } from 'src/app/Model/Member';
import { Pagination } from 'src/app/Model/pagination';
import { MemberService } from 'src/app/Service/Member/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  member$:Member[] |null;
  pagination:Pagination;
  pageNumber=1;
  pageSize=10;

  constructor(private _userService:MemberService, private _route:Router, private _toaster:ToastrService) { }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    this._userService.GetAll(this.pageNumber,this.pageSize).subscribe(res=>{
      this.member$=res.result;
      this.pagination=res.pagination;
    })
  }

  pageChanged(event:any){
    this.pageNumber=event.page;
    this.loadMember();
  }


}
